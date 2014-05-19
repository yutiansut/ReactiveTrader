
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.Concurrency;
using System.Linq;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	//[Register("PriceTilesViewController")]
	public partial class PriceTilesViewController : UITableViewController
	{
		private readonly IReactiveTrader _reactiveTrader;
		private readonly IConcurrencyService _concurrencyService;
		private readonly PriceTilesModel _model;

		public PriceTilesViewController (IReactiveTrader reactiveTrader, IConcurrencyService concurrencyService) 
			: base(UITableViewStyle.Plain)
		{
			this._concurrencyService = concurrencyService;
			this._reactiveTrader = reactiveTrader;

			Title = "Prices";
			TabBarItem.Image = UIImage.FromBundle ("adaptive");

			_model = new PriceTilesModel (_reactiveTrader, _concurrencyService);

			_model.ActiveCurrencyPairs.CollectionChanged += (sender, e) => {
				foreach (var model in e.NewItems.Cast<PriceTileModel>()) {
					model.OnChanged
						.Subscribe (OnItemChanged);
				}
				if (IsViewLoaded) {
					TableView.ReloadData ();
				}
			};
			_model.Initialise ();

		}

		private void OnItemChanged(PriceTileModel item) {

			if (IsViewLoaded) {
				var indexOfItem = _model.ActiveCurrencyPairs.IndexOf (item);

				TableView.ReloadRows (
					new [] {
						NSIndexPath.Create (0, indexOfItem)
					}, UITableViewRowAnimation.None);
			}
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
			TableView.Source = new PriceTilesViewSource (_model);
			//			TableView.Delegate = new PriceTilesViewDelegate ();

			// TODO: Obtain Adaptive branding RGB value to use here.
			TableView.BackgroundColor = UIColor.FromRGB (10, 15, 30);
		}
	}
}

