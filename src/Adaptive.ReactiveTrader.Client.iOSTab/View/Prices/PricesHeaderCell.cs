using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public partial class PricesHeaderCell : UITableViewHeaderFooterView
	{
		public static readonly UINib Nib = UINib.FromName ("PricesHeaderCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("PricesHeaderCell");

		private UserModel _currentUserModel;

		public PricesHeaderCell (IntPtr handle) : base (handle)
		{
		}

		public static PricesHeaderCell Create ()
		{
			return (PricesHeaderCell)Nib.Instantiate (null, null) [0];
		}

		private void DecorateWithEnabledness(Boolean isEnabled)
		{
			if (this.StatusSwitch.On != isEnabled) {
				this.StatusSwitch.On = isEnabled;
			}

			if (isEnabled) {
				this.ContentView.BackgroundColor = Styles.RTWarnOrange;
				this.StatusLabel.Text = "One touch trading is enabled";
			} else {
				this.ContentView.BackgroundColor = Styles.RTDarkerBlue;
				this.StatusLabel.Text = "One touch trading is disabled";
			}
		}

		public void UpdateFrom (UserModel userModel)
		{
			userModel.OnChanged
				.Subscribe (OnItemChanged);
			this._currentUserModel = userModel;
			this.DecorateWithEnabledness (userModel.GetOneTouchTradingEnabled());
		}

		private void OnItemChanged(UserModel item) {
			DecorateWithEnabledness(item.GetOneTouchTradingEnabled());
		}

		partial void SwitchValueChanged (MonoTouch.Foundation.NSObject sender)
		{
			if (this._currentUserModel != null) {
				UISwitch asSwitch = (UISwitch)sender;
				this._currentUserModel.SetOneTouchTradingEnabled(asSwitch.On);
			}
		}
	}
}

