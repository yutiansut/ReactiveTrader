using System;
using System.Collections.ObjectModel;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Shared.Extensions;
using Android.App;
using Android.Views;
using Android.Widget;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class SpotTileAdapter : BaseAdapter<ISpotTileViewModel>
    {
        private readonly Activity _context;
        private readonly ObservableCollection<ISpotTileViewModel> _spotTileCollecton;

        private readonly IDisposable _collectionChangedSubscription;

        public SpotTileAdapter(Activity context, ObservableCollection<ISpotTileViewModel> spotTileCollecton)
        {
            _context = context;
            _spotTileCollecton = spotTileCollecton;

            _collectionChangedSubscription = _spotTileCollecton.ObserveCollection()
                .Subscribe(_ =>
                {
                    NotifyDataSetChanged();
                });
        }

        public override ISpotTileViewModel this[int position]
        {
            get
            {
                return _spotTileCollecton[position];
            }
        }

        public override int Count
        {
            get
            {
                return _spotTileCollecton.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.SpotTileView, parent, false);

                var currencyPairLabel = view.FindViewById<TextView>(Resource.Id.SpotTileCurrencyPairTextView);

                view.Tag = new SpotTileViewHolder { CurrencyPairLabel = currencyPairLabel };
            }

            var holder = (SpotTileViewHolder)view.Tag;
            holder.CurrencyPairLabel.Text = _spotTileCollecton[position].CurrencyPair;

            return view;
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