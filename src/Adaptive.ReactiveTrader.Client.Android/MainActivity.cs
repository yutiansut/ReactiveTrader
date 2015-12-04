using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adaptive.ReactiveTrader.Client.Android.UI.Blotter;
using Adaptive.ReactiveTrader.Client.Android.UI.Prices;
using Adaptive.ReactiveTrader.Client.Android.UI.Status;
using Adaptive.ReactiveTrader.Client.Concurrency;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.UI.Shell;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Autofac;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using String = Java.Lang.String;

namespace Adaptive.ReactiveTrader.Client.Android
{
    [Activity(MainLauncher=true, Icon="@drawable/icon", Label="Reactive Trader", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainActivity);

            var adapter = new TabsAdapter(this, SupportFragmentManager);
            var viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            viewPager.Adapter = adapter;
           // viewPager.SetCurrentItem(0, true);

           // var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.tabLayout);
            tabLayout.SetupWithViewPager(viewPager);
        }

        void AddTab(string text)
        {
            var tab = ActionBar.NewTab();
            tab.SetText(text);
            tab.TabSelected += (sender, args) => { };
            ActionBar.AddTab(tab);
        }
    }

    class TabsAdapter : FragmentPagerAdapter
    {
        private readonly Context _context;
        readonly string[] _tabName = { "Prices", "Trades", "Status" };
        readonly Fragment[] _fragments =
        {
            new PricesListFragment(App.Container.Resolve<IShellViewModel>()),
            new BlotterFragment(App.Container.Resolve<IShellViewModel>()),
            new StatusFragment(App.Container.Resolve<IReactiveTrader>(), App.Container.Resolve<IConcurrencyService>())
        };

        public TabsAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public TabsAdapter(Context context, FragmentManager fm) : base(fm)
        {
            _context = context;
        }

        public override int Count => _fragments.Length;

        public override Fragment GetItem(int position)
        {
            return _fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new String(_tabName[position]);
        }
    }
}