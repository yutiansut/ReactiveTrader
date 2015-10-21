using System;
using Foundation;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using UIKit;

namespace Adaptive.ReactiveTrader.Client.iOSTab.WatchKitExtension
{
    public partial class RowController : NSObject
    {
        public RowController () 
        {
        }

        public RowController (IntPtr ptr) : base(ptr) 
        {
        }

        IDisposable _subscription = Disposable.Empty;

        ICurrencyPair _currencyPair;

        public ICurrencyPair CurrencyPair
        {
            get
            {
                return _currencyPair;
            }

            set
            {
                _currencyPair = value;

                _subscription.Dispose();
                _subscription = _currencyPair.PriceStream
                    .Where(price => !price.IsStale)
                    .SubscribeOn(new EventLoopScheduler()).Subscribe(price => 
                    {
                        var sellPrice = PriceFormatter.GetFormattedPrice (price.Bid.Rate, price.CurrencyPair.RatePrecision, price.CurrencyPair.PipsPosition);
                        var buyPrice = PriceFormatter.GetFormattedPrice (price.Ask.Rate, price.CurrencyPair.RatePrecision, price.CurrencyPair.PipsPosition);


                        _sellLabel.SetText(sellPrice.ToAttributedString());
                        _buyLabel.SetText(buyPrice.ToAttributedString());

                        
                    });
                

                _currencyPair.PriceStream
                    .Where(price => !price.IsStale)
                    .Scan(0m, (last, price) => price.Mid - last)
                    .Select(delta =>
                        {
                            if (delta > 0)
                            {
                                return PriceMovement.Up;
                            }

                            if (delta < 0)
                            {
                                return PriceMovement.Down;
                            }

                            return PriceMovement.None;
                        }
                    
                    ).Subscribe(movement =>
                    {
                        // ▲  ▼

                        switch (movement)
                        {
                            case PriceMovement.None:
                                Label.SetText(_currencyPair.BaseCurrency + " / " + _currencyPair.CounterCurrency);
                                break;
                            case PriceMovement.Down:
                                Label.SetText(_currencyPair.BaseCurrency + " / " + _currencyPair.CounterCurrency + " ▼");
                                break;
                            case PriceMovement.Up:
                                Label.SetText(_currencyPair.BaseCurrency + " / " + _currencyPair.CounterCurrency + " ▲");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
                        }
                    });


                //_currencyPair.PriceStream.Aggregate()
                UpdateCell();
            }
        }

        private void UpdateCell()
        {
            Label.SetText(_currencyPair.BaseCurrency + " / " + _currencyPair.CounterCurrency);
        }
    }


    public static class PriceExtentions
    {
        public static NSAttributedString ToAttributedString(this UI.SpotTiles.FormattedPrice price)
        {
            var str = new NSMutableAttributedString(price.BigFigures);
            str.Append(new NSAttributedString(price.Pips, UIFont.BoldSystemFontOfSize(1)));
            str.Append(new NSAttributedString(price.TenthOfPip));

            return str;
        }
    }
}

