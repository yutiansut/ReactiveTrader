using System;
using System.Collections.ObjectModel;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Shared.Extensions;
using Android.Support.V7.Widget;
using Android.Views;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class SpotTileAdapter : RecyclerView.Adapter
    {
        private readonly ObservableCollection<ISpotTileViewModel> _spotTileCollecton;

        private readonly IDisposable _collectionChangedSubscription;

        public SpotTileAdapter(ObservableCollection<ISpotTileViewModel> spotTileCollecton)
        {
            _spotTileCollecton = spotTileCollecton;

            _collectionChangedSubscription = _spotTileCollecton.ObserveCollection()
                .Subscribe(_ =>
                {
                    NotifyDataSetChanged();
                });
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var h = (SpotTileViewHolder)holder;
            h.CurrencyPairLabel.Text = _spotTileCollecton[position].CurrencyPair;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SpotTileView, parent, false);
            var holder = new SpotTileViewHolder(v);
            return holder;
        }

        public override int ItemCount
        {
            get { return _spotTileCollecton.Count; }
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