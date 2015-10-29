using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Domain.Models.Pricing;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Foundation;
using UIKit;
using WatchKit;
using System.Reactive.Concurrency;
using Adaptive.ReactiveTrader.Client.Domain.Models.Execution;

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

        readonly static UIFont _arrowFont = UIFont.SystemFontOfSize(20);
        readonly static Dictionary<PriceMovement, NSAttributedString> _movementText = new Dictionary<PriceMovement, NSAttributedString>
        {
            {PriceMovement.Up,   new NSAttributedString("▲", _arrowFont, UIColor.Green) },
            {PriceMovement.Down, new NSAttributedString("▼", _arrowFont, UIColor.Red) },
            {PriceMovement.None, new NSAttributedString("▼", _arrowFont, UIColor.Black) }
        };

        public override void Awake(NSObject context)
        {
            base.Awake(context);

            _pair = PairFromContext(context);

            if (_pair == null)
            {
                return;
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

            var buffered = _pair.PriceStream.Where(price => !price.IsStale); //.Sample(TimeSpan.FromSeconds(.1));
                
            _subscription = new CompositeDisposable
            {
                buffered.Subscribe(price => _price = price),

                buffered
                    .Where(_ => !_executingSell)
                    .Select(price => price.ToBidPrice().ToAttributedString())
                    .Subscribe(SellPriceLabel.SetText),

                buffered
                    .Where(_ => !_executingBuy)
                    .Select(price => price.ToAskPrice().ToAttributedString())
                    .Subscribe(BuyPriceLabel.SetText),

                buffered
                    .ToPriceMovementStream()
                    .DistinctUntilChanged()
                    .Select(movement => 
                        {
                            var spread = new NSAttributedString(_price.Spread.ToString("0.0") + "   ");
                            var text = new NSMutableAttributedString(spread);
                            text.Append(_movementText[movement]);
                            return text;
                        })
                    .Subscribe(PriceLabel.SetText)                    
            };
        }

        public override void DidDeactivate()
        {
            _subscription.Dispose();
            base.DidDeactivate();
        }

        partial void SellButtonTapped()
        {
            SellPriceLabel.SetText("Executing...");
            _executingSell = true;

            _price.Bid
                .ExecuteRequest(50000, _pair.BaseCurrency)
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
            
            BuyPriceLabel.SetText("Executing...");
            _executingSell = true;

            _price.Ask
                .ExecuteRequest(50000, _pair.BaseCurrency)
                .Where(result => !result.IsStale)
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
