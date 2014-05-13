

using System.Collections.Generic;
using Adaptive.ReactiveTrader.Client.Domain.Models;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.Concurrency;
using System.IO;
using Adaptive.ReactiveTrader.Client.iOSTab.Tiles;
using Adaptive.ReactiveTrader.Shared.UI;


namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public class PriceTilesViewSource : UITableViewSource
	{
		private readonly PriceTilesModel priceTilesModel;

		public PriceTilesViewSource (PriceTilesModel priceTilesModel)
		{
			this.priceTilesModel = priceTilesModel;
		}


		public override int NumberOfSections (UITableView tableView)
		{
			// TODO: return the actual number of sections
			return 1;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return priceTilesModel.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			PriceTileModel model = priceTilesModel [(int)indexPath.IndexAtPosition (1)];

			var cell = GetCell (tableView, model);

			cell.UpdateFrom (model);

			return ( UITableViewCell)cell;
		}

		private IPriceTileCell GetCell(UITableView tableView, PriceTileModel model) {
			IPriceTileCell priceTileCell = null;

			switch (model.Status) {
			case PriceTileStatus.Done:
				priceTileCell = tableView.DequeueReusableCell (PriceTileTradeAffirmationViewCell.Key) as PriceTileTradeAffirmationViewCell;
				if (priceTileCell == null)
					priceTileCell = PriceTileTradeAffirmationViewCell.Create ();


				break;
			case PriceTileStatus.Streaming:
			case PriceTileStatus.Executing:
				priceTileCell = tableView.DequeueReusableCell (PriceTileViewCell.Key) as PriceTileViewCell;
				if (priceTileCell == null)
					priceTileCell = PriceTileViewCell.Create ();

				break;
			}

			return priceTileCell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)

		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
			return 206.0f;
		}
	}
}

