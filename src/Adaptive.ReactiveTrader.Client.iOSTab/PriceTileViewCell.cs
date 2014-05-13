
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Shared.DTO.ReferenceData;
using System.ComponentModel;
using MonoTouch.CoreGraphics;
using Adaptive.ReactiveTrader.Client.iOSTab.Tiles;
using System.IO;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public partial class PriceTileViewCell : UITableViewCell, IPriceTileCell
	{
		public static readonly UINib Nib = UINib.FromName ("PriceTileViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("PriceTileViewCell");

		private PriceTileModel _priceTileMode;

		public PriceTileViewCell (IntPtr handle) : base (handle)
		{
		}

		public static PriceTileViewCell Create ()
		{
			return (PriceTileViewCell)Nib.Instantiate (null, null) [0];
		}
			
		public void UpdateFrom (PriceTileModel model)
		{
			_priceTileMode = model;

			this.CurrencyPair.Text = model.Symbol;

			this.LeftSideAction.Text = "SELL";

			this.RightSideAction.Text = "BUY";

			this.LeftSideNumber.Text = model.LeftSideNumber;
			this.LeftSideBigNumber.Text = model.LeftSideBigNumber;
			this.LeftSidePips.Text = model.LeftSidePips;

			this.RightSideNumber.Text = model.RightSideNumber;
			this.RightSideBigNumber.Text = model.RightSideBigNumber;
			this.RightSidePips.Text = model.RightSidePips;

			this.Notional.Text = model.Notional;

			this.Executing.Hidden = model.Status != PriceTileStatus.Executing;
		}

		partial void LeftSideButtonTouchUpInside (NSObject sender)
		{
			var model = _priceTileMode;
			if (model != null)
				model.Bid();
		}

		partial void RightSideButtonTouchUpInside (NSObject sender)
		{
			var model = _priceTileMode;
			if (model != null)
				model.Ask();
		}

		partial void NotionalValueChanged (NSObject sender)
		{
			var model = _priceTileMode;
			if (model != null)
				model.Notional = Notional.Text;
		}
	}
}

