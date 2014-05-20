using System;
using MonoTouch.UIKit;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public static class Styles
	{
		public static readonly UIColor RTLightBlue = UIColor.FromRGB (98, 127, 152);

		public static void ConfigureTable(UITableView uiTableView) {
			uiTableView.RowHeight = 206.0f;
			uiTableView.AllowsSelection = false;
			uiTableView.SeparatorColor = Styles.RTLightBlue;
		}
	}
}

