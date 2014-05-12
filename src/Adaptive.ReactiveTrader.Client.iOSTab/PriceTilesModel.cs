using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Domain;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using Adaptive.ReactiveTrader.Client.Concurrency;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public class PriceTilesModel : IObserver<IEnumerable<ICurrencyPairUpdate>>
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
				.Subscribe (this);
		}

		#region IObserver implementation

		void IObserver<IEnumerable<ICurrencyPairUpdate>>.OnCompleted ()
		{

		}

		void IObserver<IEnumerable<ICurrencyPairUpdate>>.OnError (Exception error)
		{

		}

		void IObserver<IEnumerable<ICurrencyPairUpdate>>.OnNext (IEnumerable<ICurrencyPairUpdate> updates)
		{
				foreach (var update in updates) {
					if (update.UpdateType == Adaptive.ReactiveTrader.Client.Domain.Models.UpdateType.Add) {

						var tileModel = new PriceTileModel (update.CurrencyPair);
						_activeCurrencyPairs.Add (tileModel);

					} else {
						// todo handle removal of price tile
					}
				}
				tableView.ReloadData ();
		}		
	
		#endregion

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
	}
}

