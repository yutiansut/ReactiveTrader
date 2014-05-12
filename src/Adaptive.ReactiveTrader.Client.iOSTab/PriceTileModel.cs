using System;
using System.Linq;
using System.Reactive.Linq;
using MonoTouch.Foundation;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using Adaptive.ReactiveTrader.Client.Domain.Models.Pricing;
using Adaptive.ReactiveTrader.Client.Concurrency;
using Adaptive.ReactiveTrader.Shared.DTO.Pricing;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public class PriceTileModel : NSObject
	{
		private readonly ICurrencyPair _currencyPair;
		private readonly IConcurrencyService _concurrencyService;
		private readonly IRedrawRequester<PriceTileModel> _redrawRequester;

		public PriceTileModel (ICurrencyPair currencyPair, IConcurrencyService concurrencyService, IRedrawRequester<PriceTileModel> redrawRequester)
		{
			this._currencyPair = currencyPair;
			this._concurrencyService = concurrencyService;
			this._redrawRequester = redrawRequester;

			this.Symbol = _currencyPair.BaseCurrency + " / " + _currencyPair.CounterCurrency;

			_currencyPair.PriceStream
				.ObserveOn(_concurrencyService.Dispatcher)
				.Subscribe (price => OnNext (price));
		}

		public string Symbol { get;	set; }

		public string LeftSideNumber { get; set; }
		public string LeftSideBigNumber { get; set; }
		public string LeftSidePips { get; set; }

		public string RightSideNumber  { get; set; }
		public string RightSideBigNumber  { get; set; }
		public string RightSidePips  { get; set; }

		void OnNext (IPrice value)
		{
			if (!value.IsStale) {
				var bid = PriceFormatter.GetFormattedPrice (value.Bid.Rate, value.CurrencyPair.RatePrecision, value.CurrencyPair.PipsPosition);
				var ask = PriceFormatter.GetFormattedPrice (value.Ask.Rate, value.CurrencyPair.RatePrecision, value.CurrencyPair.PipsPosition);

				LeftSideNumber = bid.BigFigures;
				LeftSideBigNumber = bid.Pips;
				LeftSidePips = bid.TenthOfPip;

				RightSideNumber = ask.BigFigures;
				RightSideBigNumber = ask.Pips;
				RightSidePips = ask.TenthOfPip;

				_redrawRequester.Redraw (this);
			}
		}

	}
}

