using System;
using Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles;
using Adaptive.ReactiveTrader.Client.Concurrency;
using Adaptive.ReactiveTrader.Client.UI.Shell;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;

namespace Adaptive.ReactiveTrader.Client.Android.UI.Prices
{
    public class PricesListFragment : Fragment
    {
        readonly IShellViewModel _shellViewModel;
        readonly IConcurrencyService _concurrencyService;

        public PricesListFragment() // Required by Android SDK
        {
        }

        public PricesListFragment(IShellViewModel shellViewModel, IConcurrencyService concurrencyService)
        {
            _shellViewModel = shellViewModel;
            _concurrencyService = concurrencyService;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.Prices, container, false);

            var spotTilesRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.SpotTilesRecyclerView);
            var spotTilesAdapter = new SpotTileAdapter(_shellViewModel.SpotTiles.SpotTiles, _concurrencyService);

            if (App.IsTablet)
            {
                var gridLayoutManager = new GridLayoutManager(Activity, 1);
                spotTilesRecyclerView.SetLayoutManager(gridLayoutManager);

                spotTilesRecyclerView.ViewTreeObserver.GlobalLayout +=
                    (sender, args) => SetupColumns(spotTilesRecyclerView, gridLayoutManager);
            }
            else
            {
                var layoutManager = new LinearLayoutManager(Activity);
                spotTilesRecyclerView.SetLayoutManager(layoutManager);
            }

            spotTilesRecyclerView.SetAdapter(spotTilesAdapter);
            spotTilesRecyclerView.HasFixedSize = true;

            return view;
        }

        void SetupColumns(RecyclerView recyclerView, GridLayoutManager layoutManager)
        {
            int viewWidth = recyclerView.MeasuredWidth;
            float cardViewWidth = 600;
            int newSpanCount = (int)Math.Floor(viewWidth / cardViewWidth);
            layoutManager.SpanCount = newSpanCount;
            layoutManager.RequestLayout();
        }
    }
}