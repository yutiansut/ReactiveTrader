// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace Adaptive.ReactiveTrader.Client.iOSTab.View
{
	[Register ("StatusViewController")]
	partial class StatusViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel ConnectionStatus { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel ConnectionUrl { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel ServerUpdateRate { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel UILatency { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel UIUpdateRate { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ConnectionStatus != null) {
				ConnectionStatus.Dispose ();
				ConnectionStatus = null;
			}

			if (ConnectionUrl != null) {
				ConnectionUrl.Dispose ();
				ConnectionUrl = null;
			}

			if (UIUpdateRate != null) {
				UIUpdateRate.Dispose ();
				UIUpdateRate = null;
			}

			if (ServerUpdateRate != null) {
				ServerUpdateRate.Dispose ();
				ServerUpdateRate = null;
			}

			if (UILatency != null) {
				UILatency.Dispose ();
				UILatency = null;
			}
		}
	}
}
