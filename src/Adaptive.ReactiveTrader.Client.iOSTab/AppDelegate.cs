using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Security.Principal;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.iOSTab.Logging;
using Adaptive.ReactiveTrader.Client.iOSTab.View;
using Adaptive.ReactiveTrader.Client.Domain.Transport;
using System.Runtime.InteropServices;

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
			// Appearance
			// TODO: Obtain Adaptive branding RGB value to use here.
			UITableView.Appearance.BackgroundColor = UIColor.FromRGB (10, 15, 30);
			UITableView.Appearance.SeparatorInset = UIEdgeInsets.Zero;

			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			var cs = new ConcurrencyService ();
			var logSource = new LogHub ();
			var logging = new LoggerFactory (logSource);

			#if DEBUG
			UIApplication.CheckForIllegalCrossThreadCalls = true;
			var logViewController = new LogViewController(cs, logSource);
			#endif

			_reactiveTrader = new Adaptive.ReactiveTrader.Client.Domain.ReactiveTrader ();

			// TODO: might we need to defer initialisation, in case status moves to Connected before we subscribe on ConnectionStatus?
			_reactiveTrader.Initialize ("iOS-" + Process.GetCurrentProcess ().Id, new [] { "https://reactivetrader.azurewebsites.net/signalr" }, logging);

			var tradesViewController = new TradesViewController (_reactiveTrader, cs);
			var pricesViewController = new PriceTilesViewController (_reactiveTrader, cs);
			var statusViewController = new StatusViewController (_reactiveTrader, cs);

			tabBarController = new UITabBarController ();
			tabBarController.ViewControllers = new UIViewController [] {
				pricesViewController,
				tradesViewController,
				statusViewController
				#if DEBUG
				, logViewController
				#endif
			};

			var startUpViewController = new StartUpView ();

			_reactiveTrader.ConnectionStatusStream
				.Where (ci => ci.ConnectionStatus == ConnectionStatus.Connected)
				.ObserveOn(cs.Dispatcher)
				.Subscribe (_ => startUpViewController.PresentViewController (tabBarController, false, null));

			window.RootViewController = startUpViewController;

			// make the window visible
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}

