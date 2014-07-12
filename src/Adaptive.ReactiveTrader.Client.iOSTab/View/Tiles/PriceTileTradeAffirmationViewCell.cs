
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.iOSTab.Model;
using Adaptive.ReactiveTrader.Client.Domain.Models;

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
			PriceTileTradeAffirmationViewCell created = (PriceTileTradeAffirmationViewCell)Nib.Instantiate (null, null) [0];
			created.ContentView.BackgroundColor = Styles.RTDarkerBlue;
			return created;
		}

		partial void Done (NSObject sender)
		{
			var model = _priceTileModel;
			if (model != null) {
				model.Done();
			}
		}

		public void UpdateFrom (PriceTileModel model)
		{
			_priceTileModel = model;

			var tradeDone = model.TradeDone;
			if (tradeDone == null) {
				return;
			}

			UpdateFrom (tradeDone);

			DoneButton.Hidden = false;
		}

		public void UpdateFrom(TradeDoneModel model) {

			string currencyOne = "???";
			string currencyTwo = "???";

			UIStringAttributes strikethroughAttributes;

			switch (model.Trade.TradeStatus) {
			case Domain.Models.Execution.TradeStatus.Rejected:
				strikethroughAttributes = new UIStringAttributes {
					StrikethroughStyle = NSUnderlineStyle.Single
				};
				break;

			case Domain.Models.Execution.TradeStatus.Done:
			default:
				strikethroughAttributes = new UIStringAttributes {
					StrikethroughStyle = NSUnderlineStyle.None
				};
				break;
			}

			// Always displayed 'plain'...

			Direction.Text = model.Trade.Direction.ToString ();
			TradeId.Text = model.Trade.TradeId.ToString ();

			// Displayed plain but needs some formatting to make nice...

			if (model.Trade.CurrencyPair.Length == 6) {
				currencyOne = model.Trade.CurrencyPair.Substring (0, 3);
				currencyTwo = model.Trade.CurrencyPair.Substring (3, 3);
				CurrencyPair.Text = String.Format("{0} / {1}", currencyOne, currencyTwo);
			} else {
				// We expect the currency pair to always be 3 + 3, but just in case...
				CurrencyPair.Text = model.Trade.CurrencyPair;
			}


			//			System.Console.WriteLine ("Trade details: {0}", model.Trade.ToString());

			// May be struck through in the event of trade failure...

			CounterCCY.AttributedText = new NSAttributedString(model.Trade.DealtCurrency, strikethroughAttributes);
			DirectionAmount.AttributedText = new NSAttributedString(Styles.FormatNotional(model.Trade.Notional, true), strikethroughAttributes);
			Rate.AttributedText = new NSAttributedString(model.Trade.SpotRate.ToString (), strikethroughAttributes);

			// 'Proper' trade dates not yet supports? So use this format to match web client...

			string valueDateFromTradeDate = String.Format("SP. {0}", model.Trade.TradeDate.ToShortDateString ());
			ValueDate.AttributedText = new NSAttributedString(valueDateFromTradeDate, strikethroughAttributes);

			// We use some BOLD if the trader id matches the current user...

			UIStringAttributes maybeStrikeMaybeBold = new UIStringAttributes();
			maybeStrikeMaybeBold.StrikethroughStyle = strikethroughAttributes.StrikethroughStyle;
			if (model.Trade.TraderName == UserModel.Instance.TraderId) {
				maybeStrikeMaybeBold.Font = UIFont.BoldSystemFontOfSize(TraderId.Font.PointSize);
			}

			TraderId.AttributedText = new NSAttributedString(model.Trade.TraderName, maybeStrikeMaybeBold);

			//
			// Not directly available from ITrade, so we derive it thus?
			// Previously was always 'EUR', so this better but probably not right!
			//

			//			string directionCurrency = (model.Trade.Direction == Adaptive.ReactiveTrader.Client.Domain.Models.Direction.BUY) ? currencyOne : currencyTwo;
			string directionCurrency = currencyOne;
			DirectionCCY.AttributedText = new NSAttributedString(directionCurrency, strikethroughAttributes);

			DoneButton.Hidden = true;

		}
	}
}

