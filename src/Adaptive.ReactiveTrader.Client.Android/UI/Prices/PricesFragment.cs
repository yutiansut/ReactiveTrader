using System;
using Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles;
using Adaptive.ReactiveTrader.Client.UI.Shell;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using JP.Wasabeef.Recyclerview.Animators;
using JP.Wasabeef.Recyclerview.Animators.Adapters;
using Fragment = Android.Support.V4.App.Fragment;

namespace Adaptive.ReactiveTrader.Client.Android.UI.Prices
{
    public class PricesListFragment : Fragment
    {
        readonly IShellViewModel _shellViewModel;

        public PricesListFragment(IShellViewModel shellViewModel)
        {
            _shellViewModel = shellViewModel;
        }

       
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.Prices, container, false);

            var spotTilesRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.SpotTilesRecyclerView);

            //spotTilesRecyclerView.SetItemAnimator(new ScaleInAnimator());
            
            var gridLayoutManager = new GridLayoutManager(Activity, App.IsTablet ? 2 : 1);
            spotTilesRecyclerView.SetLayoutManager(gridLayoutManager);

            var spotTilesAdapter = new SpotTileAdapter(_shellViewModel.SpotTiles.SpotTiles);
            //var animatingAdapter = new ScaleInAnimationAdapter(spotTilesAdapter);
            //animatingAdapter.SetFirstOnly(false);
            spotTilesRecyclerView.SetAdapter(spotTilesAdapter);

            //var displayMetrics = new DisplayMetrics();
            //WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            //spotTilesRecyclerView.HasFixedSize = true;
           // spotTilesRecyclerView.ViewTreeObserver.AddOnGlobalLayoutListener(new RecyclerViewSpanCalculatorLayoutListener(spotTilesRecyclerView, gridLayoutManager, displayMetrics));

            

            return view;
        }
    }
}