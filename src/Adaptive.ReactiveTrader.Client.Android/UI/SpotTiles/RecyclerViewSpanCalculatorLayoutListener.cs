using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Java.Lang;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class RecyclerViewSpanCalculatorLayoutListener : Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private readonly RecyclerView _recyclerView;
        private readonly GridLayoutManager _gridLayoutManager;
        private readonly DisplayMetrics _displayMetrics;

        public RecyclerViewSpanCalculatorLayoutListener(RecyclerView recyclerView, GridLayoutManager gridLayoutManager, DisplayMetrics displayMetrics)
        {
            _recyclerView = recyclerView;
            _gridLayoutManager = gridLayoutManager;
            _displayMetrics = displayMetrics;
        }

        public void OnGlobalLayout()
        {
            var viewWidth = _recyclerView.MeasuredWidth / _displayMetrics.Density;
            const double tileWidth = 256;
            int newSpanCount = (int)((viewWidth / tileWidth));
            _gridLayoutManager.SpanCount = (Math.Max(1, newSpanCount));
            _gridLayoutManager.RequestLayout();
        }
    }
}