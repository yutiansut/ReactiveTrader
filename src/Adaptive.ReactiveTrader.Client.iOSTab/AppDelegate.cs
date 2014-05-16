using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.Domain;
using System.Diagnostics;
using Adaptive.ReactiveTrader.Client.iOSTab.Logging;
using Adaptive.ReactiveTrader.Client.iOSTab.View;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		private IReactiveTrader _reactiveTrader;

		// class-level declarations
		UIWindow window;
		UITabBarController tabBarController;
		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			var cs = new ConcurrencyService ();
			var logSource = new LogHub ();
			var logging = new LoggerFactory (logSource);

			#if DEBUG
			var logViewController = new LogViewController(cs, logSource);
			#endif

			_reactiveTrader = new Adaptive.ReactiveTrader.Client.Domain.ReactiveTrader ();
			_reactiveTrader.Initialize ("iOS-" + Process.GetCurrentProcess ().Id, new [] { "https://reactivetrader.azurewebsites.net/signalr" }, logging);

//			_reactiveTrader.ConnectionStatusStream
//				.SubscribeOn(cs.TaskPool)
//				.ObserveOn(cs.Dispatcher)
//				.Subscribe (ci => {
//				var view = new UIAlertView () {
//					Title = "Connection Status",
//					Message = string.Format ("Reactive Trader is now {0}.", ci.ConnectionStatus.ToString ().ToLowerInvariant ())
//				};
//				view.AddButton ("OK");
//				view.Show ();
//				});

			var tradesViewController = new TradesViewController (_reactiveTrader, cs);
			var pricesViewController = new PriceTilesViewController (_reactiveTrader, cs);
			var statusViewController = new StatusViewController (_reactiveTrader, cs);
			tabBarController = new UITabBarController ();
			tabBarController.ViewControllers = new UIViewController [] {
				statusViewController,
				tradesViewController,
				pricesViewController
				#if DEBUG
				, logViewController
				#endif
			};


			window.RootViewController = tabBarController;
			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

