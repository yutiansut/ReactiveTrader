using System;
using Foundation;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using System.Reactive.Disposables;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
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
                        if (_buyLabel == null)
                        {
                            return;
                        }

                            

                        var sellPrice = PriceFormatter.GetFormattedPrice (price.Bid.Rate, price.CurrencyPair.RatePrecision, price.CurrencyPair.PipsPosition);
                        var buyPrice = PriceFormatter.GetFormattedPrice (price.Ask.Rate, price.CurrencyPair.RatePrecision, price.CurrencyPair.PipsPosition);

//                        Console.WriteLine(sellPrice.BigFigures + "." + sellPrice.Pips + "." + sellPrice.TenthOfPip);


                        var new NSAttributedString(sellPrice.Pips, UIFont.BoldSystemFontOfSize(16));

                        _sellLabel.SetText(sellPrice.BigFigures + sellPrice.Pips + sellPrice.TenthOfPip);
                        _buyLabel.SetText(buyPrice.BigFigures + buyPrice.Pips + buyPrice.TenthOfPip);

                    });
                
                UpdateCell();                    
            }
        }

        void UpdateCell()
        {
            Label.SetText(_currencyPair.BaseCurrency + " / " + _currencyPair.CounterCurrency);
        }
    }

    public static ToAttributedString(this IPrice price)
    {
        
    }
}

