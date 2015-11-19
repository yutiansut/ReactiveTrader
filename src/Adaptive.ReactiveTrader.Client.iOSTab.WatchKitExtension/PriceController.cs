using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Domain.Models.Execution;
using Adaptive.ReactiveTrader.Client.Domain.Models.Pricing;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using Adaptive.ReactiveTrader.Client.iOS.Shared;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Foundation;
using UIKit;
using WatchKit;

namespace Adaptive.ReactiveTrader.Client.iOSTab.WatchKitExtension
{
	partial class PriceController : WKInterfaceController
	{
		public PriceController (IntPtr handle) : base (handle)
		{
		}

        ICurrencyPair _pair;
        IPrice _price;
        bool _executingSell;
        bool _executingBuy;
        IDisposable _subscription = Disposable.Empty;

        public override void Awake(NSObject context)
        {
            base.Awake(context);

            _pair = PairFromContext(context);

            if (_pair == null)
            {
                return;
            }

            if (Pairs.NotificationCurrencyPair != null && Pairs.NotificationCurrencyPair.Matches(_pair))
            {
                BecomeCurrentPage();
                Pairs.NotificationCurrencyPair = null;
            }

            SetTitle($"{_pair.BaseCurrency} / {_pair.CounterCurrency}");
        }

        public override void WillActivate()
        {
            base.WillActivate();

            if (_pair == null)
            {
                return;
            }

            var stream = _pair.PriceStream;
                
            _subscription = new CompositeDisposable
            {
                stream.Subscribe(price => _price = price),

                stream
                    .Where(price => !price.IsStale && !_executingSell)
                    .Select(price => price.ToBidPrice().ToAttributedString())
                    .Subscribe(SellPriceLabel.SetText),

                stream
                    .Where(price => !price.IsStale && !_executingBuy)
                    .Select(price => price.ToAskPrice().ToAttributedString())
                    .Subscribe(BuyPriceLabel.SetText),

                stream
                    .Where(price => !price.IsStale)
                    .ToPriceMovementStream()
                    .DistinctUntilChanged()                    
                    .Select(movement => movement.ToAttributedString(_price))
                    .Subscribe(PriceLabel.SetText),

                stream
                    .Where(price => price.IsStale)
                    .Subscribe(_ => 
                        {
                            SetStale(BuyButton);
                            SetStale(SellButton);
                        }),

                stream
                    .Where(price => !price.IsStale)
                    .Subscribe(_ => 
                        {
                            SetLive(BuyButton);
                            SetLive(SellButton);
                        })
            };
        }

        void SetLive(WKInterfaceButton button)
        {
            button.SetBackgroundColor(UIColor.FromRGBA(red: 0.16f, green: 0.26f, blue: 0.4f, alpha: 1f));
            SellButton.SetEnabled(true);
        }

        void SetStale(WKInterfaceButton button)
        {
            button.SetBackgroundColor(UIColor.Red);
            button.SetTitle("-");
            button.SetEnabled(false);
        }

        public override void DidDeactivate()
        {
            _subscription.Dispose();
            base.DidDeactivate();
        }

        partial void SellButtonTapped()
        {
            var executePrice = _price;

            if (executePrice.IsStale)
            {
                return;
            }

            SellPriceLabel.SetText("Executing...");
            _executingSell = true;

            executePrice.Bid.ExecuteRequest(50000, _pair.BaseCurrency)
                .Where(result => !result.IsStale)
                .Subscribe(result =>             
                {
                    Console.WriteLine("Executed");
                    _executingSell = false;
                    ShowConfirmation(result.Update);
                });
        }

        partial void BuyButtonTapped()
        {                       
            var executePrice = _price;

            if (executePrice.IsStale)
            {
                return;
            }

            BuyPriceLabel.SetText("Executing...");
            _executingBuy = true;

            executePrice.Ask.ExecuteRequest(50000, _pair.BaseCurrency)
                .Subscribe(result => 
                {
                    Console.WriteLine("Executed");
                    _executingBuy = false;
                    ShowConfirmation(result.Update);
                });
        }

        void ShowConfirmation(ITrade trade)
        {
            Trades.Shared[trade.TradeId] = trade;
                 
            InvokeOnMainThread(() => 
                PresentController(TradeConfirmController.Name, new NSNumber(trade.TradeId))
            );
        }

        ICurrencyPair PairFromContext(NSObject context)
        {
            if (context == null)
            {
                return null;
            }

            var index = ((NSNumber)context).Int32Value;

            if (Pairs.Shared.Count < index)
            {
                Console.WriteLine("No pair");
                return null;
            }

            return Pairs.Shared[index];
        }
	}
}
