
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Shared.DTO.ReferenceData;
using System.ComponentModel;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public partial class PriceTileViewCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("PriceTileViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("PriceTileViewCell");


		public PriceTileViewCell (IntPtr handle) : base (handle)
		{
		}

		public static PriceTileViewCell Create ()
		{
			return (PriceTileViewCell)Nib.Instantiate (null, null) [0];
		}

		public void UpdateFrom (PriceTileModel model)
		{
			this.CurrencyPair.Text = model.Symbol;

			this.LeftSideAction.Text = "SELL";

			this.RightSideAction.Text = "BUY";
			/*
			[Outlet]
			MonoTouch.UIKit.UILabel LeftSideBigNumber { get; set; }

			[Outlet]
			MonoTouch.UIKit.UILabel LeftSideNumber { get; set; }

			[Outlet]
			MonoTouch.UIKit.UILabel LeftSidePips { get; set; }

			[Outlet]
			MonoTouch.UIKit.UITextField Notional { get; set; }

			[Outlet]
			MonoTouch.UIKit.UILabel NotionalCcy { get; set; }

			[Outlet]
			MonoTouch.UIKit.UIImageView PriceMovementDown { get; set; }

			[Outlet]
			MonoTouch.UIKit.UIImageView PriceMovementUp { get; set; }

			[Outlet]
			MonoTouch.UIKit.UILabel RightSideAction { get; set; }

			[Outlet]
			MonoTouch.UIKit.UILabel RightSideBigNumber { get; set; }

			[Outlet]
			MonoTouch.UIKit.UILabel RightSideNumber { get; set; }

			[Outlet]
			MonoTouch.UIKit.UILabel RightSidePips { get; set; }

			[Outlet]
			MonoTouch.UIKit.UILabel SpotDate { get; set; }

			[Outlet]
			MonoTouch.UIKit.UILabel Spread { get; set; }
*/
		}

		partial void LeftSideButtonTouchUpInside (NSObject sender)
		{

		}

		partial void RightSideButtonTouchUpInside (NSObject sender)
		{

		}

		partial void NotionalValueChanged (NSObject sender)
		{

		}
	}
}

