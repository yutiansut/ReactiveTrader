using System;
using WatchKit;
using Foundation;
using UIKit;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Domain.Transport;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Client.Domain.Models.Pricing;
using Adaptive.ReactiveTrader.Client.iOS.Shared;
using Adaptive.ReactiveTrader.Client.Domain.Models.Execution;

namespace Adaptive.ReactiveTrader.Client.iOSTab.WatchKitExtension
{
    public partial class NotificationController : WKUserNotificationInterfaceController
    {
        Adaptive.ReactiveTrader.Client.Domain.ReactiveTrader _reactiveTrader;
        IPrice _price;
        ITrade _trade;

        public override void DidReceiveLocalNotification(UILocalNotification localNotification, Action<WKUserNotificationInterfaceType> completionHandler)
        {
            _reactiveTrader = new Adaptive.ReactiveTrader.Client.Domain.ReactiveTrader();
            _reactiveTrader.Initialize (UserModel.Instance.TraderId, new [] { "https://reactivetrader.azurewebsites.net/signalr" });
            _reactiveTrader.ConnectionStatusStream
                .Where(ci => ci.ConnectionStatus == ConnectionStatus.Connected)
                .Timeout(TimeSpan.FromSeconds(5))
                .Subscribe(
                    _ => completionHandler(WKUserNotificationInterfaceType.Custom),
                    ex => completionHandler(WKUserNotificationInterfaceType.Custom)
                );
            
            var tradeJson = (NSString)localNotification.UserInfo.ValueForKey((NSString)"trade");

            _trade = tradeJson.ToTrade();
            _tradeDetailsLabel1.SetText(_trade.ToAttributedStringLine1());
            _tradeDetailsLabel2.SetText(_trade.ToAttributedStringLine2());


            var priceStreams = _reactiveTrader.ReferenceData
                .GetCurrencyPairsStream()
                .SelectMany(pairs => pairs)
                .Where(update => _trade.CurrencyPair == update.CurrencyPair.BaseCurrency + update.CurrencyPair.CounterCurrency)
                .Select(update => update.CurrencyPair.PriceStream)
                .Switch();

            priceStreams.Subscribe(price =>
                {
                    _price = price;
                    _sellPriceLabel.SetText(price.ToBidPrice().ToAttributedNotificationString());
                    _buyPriceLabel.SetText(price.ToAskPrice().ToAttributedNotificationString());
                });

            priceStreams.ToPriceMovementStream().Subscribe(
                priceMovement => 
                {
                    _arrowLabel.SetText(priceMovement.ToAttributedArrow(_price));
                    _priceLabel.SetText(_price.Spread.ToString("0.0"));
                    SetPricesHidden(false);
                }
            );

        }

        void SetPricesHidden(bool hidden)
        {
            _priceGroup.SetAlpha(hidden ? 0 : 1);
        }

        public override void DidDeactivate()
        {
            // This method is called when the watch view controller is no longer visible to the user.
            Console.WriteLine("{0} did deactivate", this);
            _reactiveTrader.Dispose();
        }
    }

}

