
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.iOSTab.Model;

namespace Adaptive.ReactiveTrader.Client.iOSTab.Tiles
{
	public partial class PriceTileTradeAffirmationViewCell : UITableViewCell, IPriceTileCell, ITradeDoneCell
	{
		public static readonly UINib Nib = UINib.FromName ("PriceTileTradeAffirmationViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("PriceTileTradeAffirmationViewCell");

		PriceTileModel _priceTileModel;

		public PriceTileTradeAffirmationViewCell (IntPtr handle) : base (handle)
		{
		}

		public static PriceTileTradeAffirmationViewCell Create ()
		{
			return (PriceTileTradeAffirmationViewCell)Nib.Instantiate (null, null) [0];
		}

		partial void Done (NSObject sender)
		{
			var model = _priceTileModel;
			if (model != null)
				model.Done();
		}

		public void UpdateFrom (PriceTileModel model)
		{
			_priceTileModel = model;

			var tradeDone = model.TradeDone;
			if (tradeDone == null)
				return;

			UpdateFrom (tradeDone);

			DoneButton.Hidden = false;
		}

		public void UpdateFrom(TradeDoneModel model) {

			CurrencyPair.Text = model.Trade.CurrencyPair;

			CounterCCY.Text = model.Trade.DealtCurrency;

			Direction.Text = model.Trade.Direction.ToString();

			DirectionAmount.Text = model.Trade.Notional.ToString ();

			Rate.Text = model.Trade.SpotRate.ToString ();
			TradeId.Text = model.Trade.TradeId.ToString ();
			SpotDate.Text = model.Trade.ValueDate.ToShortDateString ();


			DoneButton.Hidden = true;

			//			[Outlet]
			//			MonoTouch.UIKit.UILabel DirectionCCY { get; set; }
		}
	}
}

