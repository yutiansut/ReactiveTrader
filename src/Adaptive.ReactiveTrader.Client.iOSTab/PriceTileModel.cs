using System;
using MonoTouch.Foundation;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public class PriceTileModel : NSObject
	{
		private readonly ICurrencyPair _currencyPair;

		public PriceTileModel (ICurrencyPair currencyPair)
		{
			this._currencyPair = currencyPair;

			this.Symbol = _currencyPair.Symbol;
		}

		public string Symbol { get;	set; }
	}
}

