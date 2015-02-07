using System;
using Adaptive.ReactiveTrader.Client.Android.Concurrency;
using Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles;
using Adaptive.ReactiveTrader.Client.Concurrency;
using Adaptive.ReactiveTrader.Client.Configuration;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.UI.Shell;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using Autofac;

namespace Adaptive.ReactiveTrader.Client.Android
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
    public class ShellActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ShellView);

            var bootstrapper = new Bootstrapper(this);
            var container = bootstrapper.Build();

            var reactiveTraderApi = container.Resolve<IReactiveTrader>();
            var username = container.Resolve<IUserProvider>().Username;
            reactiveTraderApi.Initialize(username, container.Resolve<IConfigurationProvider>().Servers);

            var shellViewModel = container.Resolve<IShellViewModel>();

            var spotTilesRecyclerView = FindViewById<RecyclerView>(Resource.Id.SpotTilesRecyclerView);
            spotTilesRecyclerView.HasFixedSize = true;

            var gridLayoutManager = new GridLayoutManager(this, 1);
            spotTilesRecyclerView.SetLayoutManager(gridLayoutManager);

            spotTilesRecyclerView.ViewTreeObserver.AddOnGlobalLayoutListener(new RecyclerViewSpanCalculatorLayoutListener(spotTilesRecyclerView, gridLayoutManager));


            var spotTilesAdapter = new SpotTileAdapter(shellViewModel.SpotTiles.SpotTiles);
            spotTilesRecyclerView.SetAdapter(spotTilesAdapter);
        }
    }
}

