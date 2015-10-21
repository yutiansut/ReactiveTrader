// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

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

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		WatchKit.WKInterfaceLabel _directionLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		WatchKit.WKInterfaceLabel CurrenciesLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		WatchKit.WKInterfaceLabel DirectionLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (_buyLabel != null) {
				_buyLabel.Dispose ();
				_buyLabel = null;
			}
			if (_directionLabel != null) {
				_directionLabel.Dispose ();
				_directionLabel = null;
			}
			if (_sellLabel != null) {
				_sellLabel.Dispose ();
				_sellLabel = null;
			}
			if (CurrenciesLabel != null) {
				CurrenciesLabel.Dispose ();
				CurrenciesLabel = null;
			}
			if (DirectionLabel != null) {
				DirectionLabel.Dispose ();
				DirectionLabel = null;
			}
		}
	}
}
