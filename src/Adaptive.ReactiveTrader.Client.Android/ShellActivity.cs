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
using Autofac;

namespace Adaptive.ReactiveTrader.Client.Android
{
    [Activity(Label = "Adaptive.ReactiveTrader.Client.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class ShellActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ShellView);


            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Build();
            var concurrencyService = (ConcurrencyService) container.Resolve<IConcurrencyService>();
            concurrencyService.Dispatcher = new AndroidUiScheduler(this);

            var reactiveTraderApi = container.Resolve<IReactiveTrader>();
            var username = container.Resolve<IUserProvider>().Username;
            reactiveTraderApi.Initialize(username, container.Resolve<IConfigurationProvider>().Servers);

            var shellViewModel = container.Resolve<IShellViewModel>();

            var spotTilesListView = FindViewById<ListView>(Resource.Id.SpotTilesListView);
            var spotTilesAdapter = new SpotTileAdapter(this, shellViewModel.SpotTiles.SpotTiles);
            spotTilesListView.Adapter = spotTilesAdapter;
        }
    }
}

