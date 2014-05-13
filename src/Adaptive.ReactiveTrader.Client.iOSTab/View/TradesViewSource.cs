
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.iOSTab.Tiles;
using Adaptive.ReactiveTrader.Client.iOSTab.Model;

namespace Adaptive.ReactiveTrader.Client.iOSTab.View
{
	public class TradesViewSource : UITableViewSource
	{
		private readonly TradeTilesModel _tradeTilesModel;

		public TradesViewSource (TradeTilesModel tradeTilesModel)
		{
			_tradeTilesModel = tradeTilesModel;
		}

		public override int NumberOfSections (UITableView tableView)
		{
			// TODO: return the actual number of sections
			return 1;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			// TODO: return the actual number of items in the section
			return _tradeTilesModel.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (PriceTileTradeAffirmationViewCell.Key) as PriceTileTradeAffirmationViewCell;
			if (cell == null)
				cell = PriceTileTradeAffirmationViewCell.Create ();

			var doneTrade = _tradeTilesModel [(int)indexPath.IndexAtPosition (0)];

			cell.UpdateFrom (doneTrade);

			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 206.0f;
		}
	}
}

