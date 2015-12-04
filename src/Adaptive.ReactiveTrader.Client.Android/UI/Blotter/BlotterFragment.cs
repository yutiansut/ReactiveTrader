using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adaptive.ReactiveTrader.Client.UI.Shell;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;

namespace Adaptive.ReactiveTrader.Client.Android.UI.Blotter
{
    public class BlotterFragment : Fragment
    {
        readonly IShellViewModel _shellViewModel;


        public BlotterFragment(IShellViewModel shellViewModel)
        {
            _shellViewModel = shellViewModel;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.BlotterView, container, false);

            var blotterRowsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.BlotterRowsRecyclerView);
            blotterRowsRecyclerView.HasFixedSize = true;
            var blotterGridLayoutManager = new LinearLayoutManager(Activity);
            blotterRowsRecyclerView.SetLayoutManager(blotterGridLayoutManager);
            var blotterRowsAdapter = new BlotterRowAdapter(_shellViewModel.Blotter.Trades);
            blotterRowsRecyclerView.SetAdapter(blotterRowsAdapter);
            
            return view;
        }
    }
}