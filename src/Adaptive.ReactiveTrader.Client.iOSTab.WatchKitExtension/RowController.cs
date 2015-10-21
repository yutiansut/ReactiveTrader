using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;

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
        
        public ICurrencyPair CurrencyPair
        {
            get
            {
                return _currencyPair;
            }

            set
            {
                _currencyPair = value;
                UpdateCell();
            }
        }
        
        CompositeDisposable _subscription = new CompositeDisposable();

        ICurrencyPair _currencyPair;

        readonly Dictionary<PriceMovement, string> _movementText = new Dictionary<PriceMovement, string>
        {
            {PriceMovement.Up, "▲"},
            {PriceMovement.Down, "▼"},
            {PriceMovement.None, ""}
        };
        
        void UpdateCell()
        {
            CurrenciesLabel.SetText(_currencyPair.BaseCurrency + " / " + _currencyPair.CounterCurrency);

            _subscription.Dispose();
            var buffered = _currencyPair.PriceStream
                .Where(price => !price.IsStale)
                //.Buffer(TimeSpan.FromMilliseconds(100))
                //.Where(x => x.Any())
                //.Select(x => x.Last())
                .SubscribeOn(new EventLoopScheduler());

            _subscription = new CompositeDisposable
                {
                    buffered
                        .Subscribe(price =>
                        {
                            var sellPrice = PriceFormatter.GetFormattedPrice(
                                price.Bid.Rate,
                                price.CurrencyPair.RatePrecision,
                                price.CurrencyPair.PipsPosition);

                            var buyPrice = PriceFormatter.GetFormattedPrice(
                                price.Ask.Rate,
                                price.CurrencyPair.RatePrecision,
                                price.CurrencyPair.PipsPosition);

                            _sellLabel.SetText(sellPrice.ToAttributedString());
                            _buyLabel.SetText(buyPrice.ToAttributedString());
                        }),



                    buffered
                        .ToPriceMovementStream()
                        .Subscribe(movement =>
                        {
                            DirectionLabel.SetText(_movementText[movement]);
                        })
                };
        }
    }
}

