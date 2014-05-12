

using System.Collections.Generic;
using Adaptive.ReactiveTrader.Client.Domain.Models;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.Concurrency;


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

		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "Header";
		}

		public override string TitleForFooter (UITableView tableView, int section)
		{
			return "Footer";
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			PriceTileViewCell cell = tableView.DequeueReusableCell (PriceTileViewCell.Key) as PriceTileViewCell;
			if (cell == null)
				cell = PriceTileViewCell.Create();

			PriceTileModel model = priceTilesModel [(int)indexPath.IndexAtPosition (1)];

			cell.UpdateFrom (model);

			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
			return 206.0f;
		}
	}
}

