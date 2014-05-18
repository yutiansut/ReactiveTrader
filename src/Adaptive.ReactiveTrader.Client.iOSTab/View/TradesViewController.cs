
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.iOSTab.Model;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.Concurrency;

namespace Adaptive.ReactiveTrader.Client.iOSTab.View
{
	public class TradesViewController : UITableViewController
	{
		private readonly IReactiveTrader _reactiveTrader;
		private readonly IConcurrencyService _concurrencyService;
		private readonly TradeTilesModel _model;

		public TradesViewController (IReactiveTrader reactiveTrader, IConcurrencyService concurrencyService) : base (UITableViewStyle.Plain)
		{
			_reactiveTrader = reactiveTrader;
			_concurrencyService = concurrencyService;

			Title = "Trades";
			TabBarItem.Image = UIImage.FromBundle ("adaptive");

			_model = new TradeTilesModel (_reactiveTrader, _concurrencyService);

			_model.DoneTrades.CollectionChanged += (sender, e) => {
				// todo - handle insertions/removals properly
				UITableView table = this.TableView;
				if (table != null)
					table.ReloadData ();
			};

			_model.Initialise ();
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
			
			TableView.Source = new TradesViewSource (_model);
		}
	}
}

