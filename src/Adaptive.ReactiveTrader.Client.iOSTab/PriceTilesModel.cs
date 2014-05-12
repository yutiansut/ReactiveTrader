using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Domain;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using Adaptive.ReactiveTrader.Client.Concurrency;
using MonoTouch.Foundation;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public class PriceTilesModel : IRedrawRequester<PriceTileModel>
	{
		private readonly IConcurrencyService concurrencyService;
		private readonly IReactiveTrader reactiveTrader;
		private readonly UITableView tableView;

		private readonly List<PriceTileModel> _activeCurrencyPairs = new List<PriceTileModel>();


		public PriceTilesModel (IReactiveTrader reactiveTrader, IConcurrencyService concurrencyService, UITableView tableView)
		{
			this.concurrencyService = concurrencyService;
			this.reactiveTrader = reactiveTrader;
			this.tableView = tableView;
		}

		public void Initialise() {

			this.reactiveTrader.ReferenceData
				.GetCurrencyPairsStream ()
				.ObserveOn(this.concurrencyService.Dispatcher)
				.Subscribe (updates => OnCurrencyPairUpdates(updates));
		}

		private void OnCurrencyPairUpdates (IEnumerable<ICurrencyPairUpdate> updates)
		{
			foreach (var update in updates) {
				if (update.UpdateType == Adaptive.ReactiveTrader.Client.Domain.Models.UpdateType.Add) {

					var tileModel = new PriceTileModel (update.CurrencyPair, this.concurrencyService, this);

					// TODO We really want our tilemodel to be notifying here.. 

					_activeCurrencyPairs.Add (tileModel);

				} else {
					// todo handle removal of price tile
				}
			}
			tableView.ReloadData ();
		}		

		public int Count {
			get {
				return _activeCurrencyPairs.Count;
			}
		}

		public PriceTileModel this[int index] {
			get { 
				return _activeCurrencyPairs [index];
			}
		}

		#region IRedrawRequester implementation

		void IRedrawRequester<PriceTileModel>.Redraw (PriceTileModel item)
		{
			var indexOfItem = _activeCurrencyPairs.IndexOf (item);

			tableView.ReloadRows (
				new [] {
					NSIndexPath.Create (0, indexOfItem)
				}, UITableViewRowAnimation.None);
		}

		#endregion
	}

	public interface IRedrawRequester<T> {
		void Redraw (T item);
	}
}

