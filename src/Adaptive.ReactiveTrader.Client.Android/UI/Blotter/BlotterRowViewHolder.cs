using System.Threading.Tasks;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V4.Content.Res;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using JP.Wasabeef.Recyclerview.Animators.Holder;

namespace Adaptive.ReactiveTrader.Client.Android.UI.Blotter
{
    public class BlotterRowViewHolder : RecyclerView.ViewHolder
    {
        public TextView TradeDate { get; private set; }
        public TextView Direction { get; private set; }
        public TextView CurrencyPair { get; private set; }
        public TextView Notional { get; private set; }
        public TextView SpotRate { get; private set; }
        public TextView Status { get; private set; }
        public TextView ValueDate { get; private set; }
        public TextView TraderName { get; private set; }
        public LinearLayout BlotterRow { get; private set; }

        public BlotterRowViewHolder(View itemView)
            : base(itemView)
        {
            TradeDate = itemView.FindViewById<TextView>(Resource.Id.BlotterRowTradeDateTextView);
            Direction = itemView.FindViewById<TextView>(Resource.Id.BlotterRowDirectionTextView);
            CurrencyPair = itemView.FindViewById<TextView>(Resource.Id.BlotterRowCurrencyPairTextView);
            Notional = itemView.FindViewById<TextView>(Resource.Id.BlotterRowNotionalTextView);
            SpotRate = itemView.FindViewById<TextView>(Resource.Id.BlotterRowSpotRateTextView);
            Status = itemView.FindViewById<TextView>(Resource.Id.BlotterRowStatusTextView);
            ValueDate = itemView.FindViewById<TextView>(Resource.Id.BlotterRowValueDateTextView);
            TraderName = itemView.FindViewById<TextView>(Resource.Id.BlotterRowTraderNameTextView);
            BlotterRow = itemView.FindViewById<LinearLayout>(Resource.Id.BlotterRow);
        }
    }
}