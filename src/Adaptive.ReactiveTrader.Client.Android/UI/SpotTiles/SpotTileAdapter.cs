using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Shared.Extensions;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Views.Animations;
using Android.Views.InputMethods;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class SpotTileAdapter : RecyclerView.Adapter
    {
        private readonly ObservableCollection<ISpotTileViewModel> _spotTileCollection;

        private readonly IDisposable _collectionChangedSubscription;
        private readonly CompositeDisposable _allSubscriptions = new CompositeDisposable();
        private int _lastPosition = -1;

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

        static void HideKeyboard(View view)
        {
            var service = (InputMethodManager)view.Context.GetSystemService(Context.InputMethodService);
            service.HideSoftInputFromWindow(view.WindowToken, 0);
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


            viewHolder.CurrencyPairLabel.Text = spotTileViewModel.Pricing.Symbol;
            viewHolder.BidButton.SetDataContext(spotTileViewModel.Pricing.Bid);
            viewHolder.AskButton.SetDataContext(spotTileViewModel.Pricing.Ask);

            viewHolder.NotionalTextBox.EditorAction += (sender, args) =>
            {
                if (args.ActionId == ImeAction.Done)
                {
                    viewHolder.NotionalTextBox.ClearFocus();
                    HideKeyboard(viewHolder.NotionalTextBox);
                }
            };


            if (position > _lastPosition)
            {
                _lastPosition = position;
                viewHolder.CardView.ScaleY = 0;
                viewHolder.CardView.ScaleX = 0;
                viewHolder.CardView
                    .Animate()
                    .SetStartDelay(position * 120)
                    .ScaleX(1).ScaleY(1)
                    .SetDuration(450)
                    .SetInterpolator(new OvershootInterpolator(3))
                    .WithLayer()
                    .Start();
            }

            _allSubscriptions.Add(spotTileViewModel.Pricing.Bid.ObserveProperty(vm => vm.IsExecuting)
                .Subscribe(s => viewHolder.AskButton.SetEnabledOverride(!s)));

            _allSubscriptions.Add(spotTileViewModel.Pricing.Ask.ObserveProperty(vm => vm.IsExecuting)
                .Subscribe(s => viewHolder.BidButton.SetEnabledOverride(!s)));

            _allSubscriptions.Add(spotTileViewModel.Pricing.ObserveProperty(vm => vm.Spread, true)
                .Subscribe(s => viewHolder.SpreadLabel.Text = s));

            _allSubscriptions.Add(spotTileViewModel.Pricing.ObserveProperty(vm => vm.DealtCurrency, true)
                .Subscribe(s => viewHolder.DealtCurrencyLabel.Text = s));

            _allSubscriptions.Add(spotTileViewModel.Pricing.ObserveProperty(vm => vm.SpotDate, true)
                .Subscribe(s => viewHolder.SpotDateLabel.Text = s));

            // two way bind the notional
            _allSubscriptions.Add(spotTileViewModel.Pricing.ObserveProperty(vm => vm.Notional, true)
                .Where(n => n != viewHolder.NotionalTextBox.Text)
                .Subscribe(s => viewHolder.NotionalTextBox.Text = s));

            _allSubscriptions.Add(Observable.FromEventPattern<TextChangedEventArgs>(
                h => viewHolder.NotionalTextBox.TextChanged += h,
                h => viewHolder.NotionalTextBox.TextChanged -= h)
                .Where(_ => spotTileViewModel.Pricing.Notional != viewHolder.NotionalTextBox.Text)
                .Subscribe(_ =>
                {
                    spotTileViewModel.Pricing.Notional = viewHolder.NotionalTextBox.Text;
                }));

            _allSubscriptions.Add(spotTileViewModel.Pricing.ObserveProperty(vm => vm.Movement, true)
                .Subscribe(m =>
                {
                    viewHolder.UpArrow.Visibility = m == PriceMovement.Up ? ViewStates.Visible : ViewStates.Invisible;
                    viewHolder.DownArrow.Visibility = m == PriceMovement.Down ? ViewStates.Visible : ViewStates.Invisible;
                }));

            _allSubscriptions.Add(spotTileViewModel.ObserveProperty(vm => vm.State, true)
                .Subscribe(async m =>
                {
                    if (m == TileState.Affirmation)
                    {
                        HideKeyboard(viewHolder.NotionalTextBox);
                        viewHolder.SetAffirmation(spotTileViewModel.Affirmation);
                        AnimationFactory.FlipTransition(viewHolder.ViewAnimator, FlipDirection.LeftRight, 200);
                        await Task.Delay(4000);
                        spotTileViewModel.DismissAffirmation();
                        AnimationFactory.FlipTransition(viewHolder.ViewAnimator, FlipDirection.RightLeft, 200);
                        viewHolder.NotionalTextBox.ClearFocus();
                    }
                }));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SpotTileView, parent, false);
            var holder = new SpotTileViewHolder(v);
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