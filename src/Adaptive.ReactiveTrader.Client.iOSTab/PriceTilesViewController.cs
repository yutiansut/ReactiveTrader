
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

			Title = "Prices";
			TabBarItem.Image = UIImage.FromBundle ("adaptive");
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

			var view = this.View as UITableView;

			view.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
			
			// Register the TableView's data source

			var model = new PriceTilesModel (_reactiveTrader, _concurrencyService, view);
			TableView.Source = new PriceTilesViewSource (model);
			model.Initialise ();
		}
	}
}

