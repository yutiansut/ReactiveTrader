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
        }
    }
}