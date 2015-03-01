using System;
using System.Reactive.Disposables;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Shared.Extensions;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class PriceButton : LinearLayout
    {
        private readonly TextView _bigFiguresTextView;
        private readonly TextView _pipsTextView;
        private readonly TextView _tenthOfPipTextView;

        private readonly SerialDisposable _propertyChangedSubscription = new SerialDisposable();

        public PriceButton(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.PriceButton, this);

            _bigFiguresTextView = FindViewById<TextView>(Resource.Id.PriceButtonBigFiguresTextView);
            _pipsTextView = FindViewById<TextView>(Resource.Id.PriceButtonPipsTextView);
            _tenthOfPipTextView = FindViewById<TextView>(Resource.Id.PriceButtonTenthOfPipTextView);
            var directionLabelTextView = FindViewById<TextView>(Resource.Id.PriceButtonDirectionTextView);

            using (var styledAttributes = context.ObtainStyledAttributes(attrs, Resource.Styleable.price_button))
            {
                var label = styledAttributes.GetString(Resource.Styleable.price_button_direction_label);
                directionLabelTextView.Text = label;
            }
        }

        public void SetDataContext(IOneWayPriceViewModel viewModel)
        {
            _propertyChangedSubscription.Disposable = viewModel.ObserveProperty().Subscribe(_ => Update(viewModel));
            Update(viewModel);
        }

        private void Update(IOneWayPriceViewModel viewModel)
        {
            _bigFiguresTextView.Text = viewModel.BigFigures;
            _pipsTextView.Text = viewModel.Pips;
            _tenthOfPipTextView.Text = viewModel.TenthOfPip;
        }

        protected override void Dispose(bool disposing)
        {
            _propertyChangedSubscription.Dispose();
        }
    }
}