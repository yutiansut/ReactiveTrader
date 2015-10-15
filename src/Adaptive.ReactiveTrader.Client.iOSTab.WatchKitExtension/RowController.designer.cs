// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Adaptive.ReactiveTrader.Client.iOSTab.WatchKitExtension
{
	[Register ("RowController")]
	partial class RowController
	{
		[Outlet]
		WatchKit.WKInterfaceLabel _buyLabel { get; set; }

		[Outlet]
		WatchKit.WKInterfaceLabel _sellLabel { get; set; }

		[Outlet]
		WatchKit.WKInterfaceLabel Label { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Label != null) {
				Label.Dispose ();
				Label = null;
			}

			if (_sellLabel != null) {
				_sellLabel.Dispose ();
				_sellLabel = null;
			}

			if (_buyLabel != null) {
				_buyLabel.Dispose ();
				_buyLabel = null;
			}
		}
	}
}
