using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Adaptive.ReactiveTrader.Client.Domain.Models.Pricing;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Foundation;

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
                Setup();
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

        int _count = 1;

        public void UpdatePrice(IPrice price)
        {
            _sellLabel.SetText(_count.ToString());
            _buyLabel.SetText(_count.ToString());
            _count++;
        }
        
        void Setup()
        {
            CurrenciesLabel.SetText(_currencyPair.BaseCurrency + " / " + _currencyPair.CounterCurrency);
        }
    }
}

