using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class SpotTileViewHolder : RecyclerView.ViewHolder
    {
        public TextView CurrencyPairLabel { get; private set; }
        public Button BidButton { get; private set; }
        public PriceButton AskButton { get; private set; }

        public SpotTileViewHolder(View itemView) 
            : base(itemView)
        {
            CurrencyPairLabel = itemView.FindViewById<TextView>(Resource.Id.SpotTileCurrencyPairTextView);
            BidButton = itemView.FindViewById<Button>(Resource.Id.SpotTileBidButton);
            AskButton = itemView.FindViewById<PriceButton>(Resource.Id.SpotTileAskPriceButton);
        }
    }
}