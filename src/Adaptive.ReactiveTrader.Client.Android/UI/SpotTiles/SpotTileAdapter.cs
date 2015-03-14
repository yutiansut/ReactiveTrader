using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Shared.Extensions;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class SpotTileAdapter : RecyclerView.Adapter
    {
        private readonly ObservableCollection<ISpotTileViewModel> _spotTileCollection;

        private readonly IDisposable _collectionChangedSubscription;
        private readonly CompositeDisposable _allSubscriptions = new CompositeDisposable();

        public SpotTileAdapter(ObservableCollection<ISpotTileViewModel> spotTileCollection)
        {
            _spotTileCollection = spotTileCollection;

            _collectionChangedSubscription = _spotTileCollection.ObserveCollection()
                .Subscribe(_ =>
                {
                    _allSubscriptions.Clear();
                    NotifyDataSetChanged(); // xamtodo - make the change details more explicit and move to some common code
                });
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var spotTileViewModel = _spotTileCollection[position];
            if (spotTileViewModel.CurrencyPair == null)
            {
                return;
            }

            var viewHolder = (SpotTileViewHolder)holder;
            viewHolder.CurrencyPairLabel.Text = spotTileViewModel.Pricing.Symbol;
            viewHolder.BidButton.SetDataContext(spotTileViewModel.Pricing.Bid);
            viewHolder.AskButton.SetDataContext(spotTileViewModel.Pricing.Ask);

            _allSubscriptions.Add(spotTileViewModel.Pricing.Bid.ObserveProperty(vm => vm.IsExecuting)
                .Subscribe(s => viewHolder.AskButton.SetEnabledOverride(!s)));

            _allSubscriptions.Add(spotTileViewModel.Pricing.Ask.ObserveProperty(vm => vm.IsExecuting)
                .Subscribe(s => viewHolder.BidButton.SetEnabledOverride(!s)));

            _allSubscriptions.Add(spotTileViewModel.Pricing.ObserveProperty(vm => vm.Spread, true)
                .Subscribe(s => viewHolder.SpreadLabel.Text = s));

            _allSubscriptions.Add(spotTileViewModel.Pricing.ObserveProperty(vm => vm.DealtCurrency, true)
                .Subscribe(s => viewHolder.DealtCurrencyLabel.Text = s));

            _allSubscriptions.Add(spotTileViewModel.Pricing.ObserveProperty(vm => vm.SpotDate, true)
                .Subscribe(s => viewHolder.SpotDateLabel.Text = s));

            // two way bind the notional
            _allSubscriptions.Add(spotTileViewModel.Pricing.ObserveProperty(vm => vm.Notional, true)
                .Where(n => n != viewHolder.NotionalTextBox.Text)
                .Subscribe(s => viewHolder.NotionalTextBox.Text = s));
            _allSubscriptions.Add(Observable.FromEventPattern<TextChangedEventArgs>(
                h => viewHolder.NotionalTextBox.TextChanged += h,
                h => viewHolder.NotionalTextBox.TextChanged -= h)
                .Where(_ => spotTileViewModel.Pricing.Notional != viewHolder.NotionalTextBox.Text)
                .Subscribe(_ =>
                {
                    spotTileViewModel.Pricing.Notional = viewHolder.NotionalTextBox.Text;
                }));

            _allSubscriptions.Add(spotTileViewModel.Pricing.ObserveProperty(vm => vm.Movement, true)
                .Subscribe(m =>
                {
                    viewHolder.UpArrow.Visibility = m == PriceMovement.Up ? ViewStates.Visible : ViewStates.Invisible;
                    viewHolder.DownArrow.Visibility = m == PriceMovement.Down ? ViewStates.Visible : ViewStates.Invisible;
                }));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SpotTileView, parent, false);
            var holder = new SpotTileViewHolder(v);
            return holder;
        }

        public override int ItemCount
        {
            get { return _spotTileCollection.Count; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _collectionChangedSubscription.Dispose();
            }
        }
    }
}