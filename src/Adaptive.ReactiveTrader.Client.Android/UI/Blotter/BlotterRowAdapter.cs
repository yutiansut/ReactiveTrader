using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive.Disposables;
using Adaptive.ReactiveTrader.Client.UI.Blotter;
using Adaptive.ReactiveTrader.Shared.Extensions;
using Android.Support.V7.Widget;
using Android.Views;

namespace Adaptive.ReactiveTrader.Client.Android.UI.Blotter
{
    public class BlotterRowAdapter : RecyclerView.Adapter
    {
        private readonly ObservableCollection<ITradeViewModel> _tradesCollection;
        private readonly IDisposable _collectionChangedSubscription;
        private readonly CompositeDisposable _allSubscriptions = new CompositeDisposable();

        public BlotterRowAdapter(ObservableCollection<ITradeViewModel> tradesCollection)
        {
            _tradesCollection = tradesCollection;

            _collectionChangedSubscription = _tradesCollection.ObserveCollection()
                .Subscribe(_ =>
                {
                    _allSubscriptions.Clear();
                    NotifyDataSetChanged(); // xamtodo - make the change details more explicit and move to some common code
                });
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var tradeViewModel = _tradesCollection[position];
            var viewHolder = (BlotterRowViewHolder)holder;
            viewHolder.TradeDate.Text = tradeViewModel.TradeDate.ToString("dd MMM yy hh:mm");
            viewHolder.Direction.Text = tradeViewModel.Direction.ToString();
            viewHolder.CurrencyPair.Text = tradeViewModel.CurrencyPair;
            viewHolder.Notional.Text = tradeViewModel.Notional;
            viewHolder.SpotRate.Text = tradeViewModel.SpotRate.ToString(CultureInfo.InvariantCulture);
            viewHolder.Status.Text = tradeViewModel.TradeStatus;
            viewHolder.ValueDate.Text = tradeViewModel.ValueDate.ToString("dd MMM yy");
            viewHolder.TraderName.Text = tradeViewModel.TraderName;
            viewHolder.TradeId.Text = tradeViewModel.TradeId;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.BlotterRowView, parent, false);
            var holder = new BlotterRowViewHolder(v);
            return holder;
        }

        public override int ItemCount
        {
            get { return _tradesCollection.Count; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _collectionChangedSubscription.Dispose();
                _allSubscriptions.Dispose();
            }
        }
    }
}