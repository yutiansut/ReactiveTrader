
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.Concurrency;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public class PriceTilesViewController : UITableViewController
	{
		IReactiveTrader _reactiveTrader;
		IConcurrencyService _concurrencyService;

		public PriceTilesViewController (IReactiveTrader reactiveTrader, IConcurrencyService concurrencyService) : base(UITableViewStyle.Grouped)
		{
			this._concurrencyService = concurrencyService;
			this._reactiveTrader = reactiveTrader;
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Register the TableView's data source

			var model = new PriceTilesModel (_reactiveTrader, _concurrencyService, this.View as UITableView);
			TableView.Source = new PriceTilesViewSource (model);
			model.Initialise ();
		}
	}
}

