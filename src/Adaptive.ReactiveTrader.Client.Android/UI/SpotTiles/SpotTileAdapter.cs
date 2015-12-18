using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Shared.Extensions;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Object = Java.Lang.Object;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class SpotTileAdapter : RecyclerView.Adapter
    {
        private readonly ObservableCollection<ISpotTileViewModel> _spotTileCollection;
        private readonly IDisposable _collectionChangedSubscription;
        private int _lastPosition = -1;
        private readonly CompositeDisposable _allSubscriptions = new CompositeDisposable();

        public SpotTileAdapter(ObservableCollection<ISpotTileViewModel> spotTileCollection)
        {
            _spotTileCollection = spotTileCollection;

            _collectionChangedSubscription = _spotTileCollection.ObserveCollection()
                .Subscribe(eventArgs =>
                {
                    _allSubscriptions.Clear();
                    NotifyDataSetChanged(); // xamtodo - make the change details more explicit and move to some common code
                });
        }

        public override void OnViewRecycled(Object holder)
        {
            var viewHolder = (SpotTileViewHolder)holder;
            viewHolder.Unbind();

            base.OnViewRecycled(holder);
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Console.WriteLine($"OnBindViewHolder {position}");
            var spotTileViewModel = _spotTileCollection[position];
            if (spotTileViewModel.CurrencyPair == null)
            {
                Console.WriteLine($"OnBindViewHolder {position} - CurrencyPair null");

                return;
            }

            var viewHolder = (SpotTileViewHolder)holder;
            viewHolder.Bind(spotTileViewModel);
            if (position > _lastPosition)
            {
                _lastPosition = position;
                viewHolder.AnimateIn(position);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SpotTileView, parent, false);
            var holder = new SpotTileViewHolder(v);
            _allSubscriptions.Add(holder);
            return holder;
        }

        public override int ItemCount => _spotTileCollection.Count;

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