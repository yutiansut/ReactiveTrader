using System.Globalization;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class SpotTileViewHolder : RecyclerView.ViewHolder
    {
        public TextView CurrencyPairLabel { get; private set; }
        public PriceButton BidButton { get; private set; }
        public PriceButton AskButton { get; private set; }
        public TextView SpreadLabel { get; private set; }
        public DirectionArrow UpArrow { get; private set; }
        public DirectionArrow DownArrow { get; private set; }
        public TextView DealtCurrencyLabel { get; private set; }
        public EditText NotionalTextBox { get; private set; }
        public TextView SpotDateLabel { get; private set; }
        public LinearLayout Content { get; private set; }
        public CardView CardView { get; private set; }
        public ViewAnimator ViewAnimator { get; private set; }


        public SpotTileViewHolder(View itemView) 
            : base(itemView)
        {
            CurrencyPairLabel = itemView.FindViewById<TextView>(Resource.Id.SpotTileCurrencyPairTextView);
            BidButton = itemView.FindViewById<PriceButton>(Resource.Id.SpotTileBidPriceButton);
            AskButton = itemView.FindViewById<PriceButton>(Resource.Id.SpotTileAskPriceButton);
            SpreadLabel = itemView.FindViewById<TextView>(Resource.Id.SpotTileSpreadTextView);
            UpArrow = itemView.FindViewById<DirectionArrow>(Resource.Id.SpotTileUpArrow);
            DownArrow = itemView.FindViewById<DirectionArrow>(Resource.Id.SpotTileDownArrow);
            DealtCurrencyLabel = itemView.FindViewById<TextView>(Resource.Id.SpotTileDealtCurrencyTextView);
            NotionalTextBox = itemView.FindViewById<EditText>(Resource.Id.SpotTileNotionalEditText);
            SpotDateLabel = itemView.FindViewById<TextView>(Resource.Id.SpotTileSpotDateTextView);
            Content = itemView.FindViewById<LinearLayout>(Resource.Id.SpotTileContent);
            CardView = itemView.FindViewById<CardView>(Resource.Id.CardView);
            ViewAnimator = itemView.FindViewById<ViewAnimator>(Resource.Id.ViewAnimator);
        }

        public void SetAffirmation(ISpotTileAffirmationViewModel vm)
        {
            ItemView.FindViewById<TextView>(Resource.Id.ConfirmDirectionTextView).Text = vm.Direction == Domain.Models.Direction.BUY ? "Bought" : "Sold";
            ItemView.FindViewById<TextView>(Resource.Id.ConfirmCurrencyPairTextView).Text = vm.CurrencyPair;
            ItemView.FindViewById<TextView>(Resource.Id.ConfirmNotionalTextView).Text = vm.Notional.ToString();
            ItemView.FindViewById<TextView>(Resource.Id.ConfirmSpotRateTextView).Text = vm.SpotRate.ToString(CultureInfo.InvariantCulture);
            ItemView.FindViewById<TextView>(Resource.Id.ConfirmStatusTextView).Text = vm.Rejected;
            ItemView.FindViewById<TextView>(Resource.Id.ConfirmTradeIdTextView).Text = vm.TradeId.ToString();


            /*
                        viewHolder.TradeDate.Text = tradeViewModel.TradeDate.ToString("dd MMM yy hh:mm");
        viewHolder.Direction.Text = tradeViewModel.Direction.ToString();
        viewHolder.CurrencyPair.Text = tradeViewModel.CurrencyPair;
        viewHolder.Notional.Text = tradeViewModel.Notional;
        viewHolder.SpotRate.Text = tradeViewModel.SpotRate.ToString(CultureInfo.InvariantCulture);
        viewHolder.Status.Text = tradeViewModel.TradeStatus;
        viewHolder.ValueDate.Text = tradeViewModel.ValueDate.ToString("dd MMM yy");
        viewHolder.TraderName.Text = tradeViewModel.TraderName;
        viewHolder.TradeId.Text = tradeViewModel.TradeId;*/
        }
    }
}