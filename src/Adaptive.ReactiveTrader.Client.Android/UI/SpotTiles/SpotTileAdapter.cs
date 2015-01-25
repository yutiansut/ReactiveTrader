using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Shared.Extensions;
using Android.Support.V7.Widget;
using Android.Views;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class SpotTileAdapter : RecyclerView.Adapter
    {
        private readonly ObservableCollection<ISpotTileViewModel> _spotTileCollection;

        private readonly IDisposable _collectionChangedSubscription;
        private readonly IDisposable _collectionDataChangedSubscription;

        public SpotTileAdapter(ObservableCollection<ISpotTileViewModel> spotTileCollection)
        {
            _spotTileCollection = spotTileCollection;

            _collectionChangedSubscription = _spotTileCollection.ObserveCollection()
                .Subscribe(_ =>
                {
                    NotifyDataSetChanged();
                });

            _collectionDataChangedSubscription = _spotTileCollection.OnItems<ISpotTileViewModel>(
                i =>
                {
                    if (i.CurrencyPair != null)
                    {
                        return Observable.Merge(i.Pricing.Bid.ObserveProperty(), i.Pricing.Ask.ObserveProperty()).Subscribe(_ => NotifyItemChanged(_spotTileCollection.IndexOf(i)));
                    }
                    else
                    {
                        return Disposable.Empty;
                    }
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
            viewHolder.CurrencyPairLabel.Text = spotTileViewModel.CurrencyPair;

            viewHolder.BidButton.Text = spotTileViewModel.Pricing.Bid.BigFigures +
                                        spotTileViewModel.Pricing.Bid.Pips +
                                        spotTileViewModel.Pricing.Bid.TenthOfPip;


            viewHolder.AskButton.Text = spotTileViewModel.Pricing.Ask.BigFigures +
                                        spotTileViewModel.Pricing.Ask.Pips +
                                        spotTileViewModel.Pricing.Ask.TenthOfPip;
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
                _collectionDataChangedSubscription.Dispose();
            }
        }
    }
}