using Adaptive.ReactiveTrader.Client.Android.UI.Blotter;
using Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles;
using Adaptive.ReactiveTrader.Client.Configuration;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.UI.Shell;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Autofac;

namespace Adaptive.ReactiveTrader.Client.Android
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class ShellActivity : Activity
    {
        private IContainer _container;
        private IShellViewModel _shellViewModel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ShellView);

            if (_container == null)
            {
                _container = Initialize();
                _shellViewModel = _container.Resolve<IShellViewModel>();
            }

            var displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(displayMetrics);

            var spotTilesRecyclerView = FindViewById<RecyclerView>(Resource.Id.SpotTilesRecyclerView);
            spotTilesRecyclerView.HasFixedSize = true;
            var gridLayoutManager = new GridLayoutManager(this, 1);
            spotTilesRecyclerView.SetLayoutManager(gridLayoutManager);
            spotTilesRecyclerView.ViewTreeObserver.AddOnGlobalLayoutListener(new RecyclerViewSpanCalculatorLayoutListener(spotTilesRecyclerView, gridLayoutManager, displayMetrics));
            var spotTilesAdapter = new SpotTileAdapter(_shellViewModel.SpotTiles.SpotTiles);
            spotTilesRecyclerView.SetAdapter(spotTilesAdapter);


            var blotterRowsRecyclerView = FindViewById<RecyclerView>(Resource.Id.BlotterRowsRecyclerView);
            blotterRowsRecyclerView.HasFixedSize = true;
            var blotterGridLayoutManager = new LinearLayoutManager(this);
            blotterRowsRecyclerView.SetLayoutManager(blotterGridLayoutManager);
            var blotterRowsAdapter = new BlotterRowAdapter(_shellViewModel.Blotter.Trades);
            blotterRowsRecyclerView.SetAdapter(blotterRowsAdapter);
        }

        private IContainer Initialize()
        {
            var bootstrapper = new Bootstrapper(this);

            var container = bootstrapper.Build();
            var reactiveTraderApi = container.Resolve<IReactiveTrader>();
            var username = container.Resolve<IUserProvider>().Username;
            var servers = container.Resolve<IConfigurationProvider>().Servers;
            reactiveTraderApi.Initialize(username, servers);

            return container;
        }
    }
}

