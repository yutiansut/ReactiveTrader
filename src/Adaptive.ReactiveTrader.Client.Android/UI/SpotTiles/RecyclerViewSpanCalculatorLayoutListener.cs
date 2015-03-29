using Android.Support.V7.Widget;
using Android.Views;
using Java.Lang;

namespace Adaptive.ReactiveTrader.Client.Android.UI.SpotTiles
{
    public class RecyclerViewSpanCalculatorLayoutListener : Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private readonly RecyclerView _recyclerView;
        private readonly GridLayoutManager _gridLayoutManager;

        public RecyclerViewSpanCalculatorLayoutListener(RecyclerView recyclerView, GridLayoutManager gridLayoutManager)
        {
            _recyclerView = recyclerView;
            _gridLayoutManager = gridLayoutManager;
        }

        public void OnGlobalLayout()
        {
            var viewWidth = _recyclerView.MeasuredWidth;
            const double tileWidth = 331;
            int newSpanCount = (int)((viewWidth / tileWidth) * (2 / 3f));
            _gridLayoutManager.SpanCount = (Math.Max(1, newSpanCount));
            _gridLayoutManager.RequestLayout();
        }
    }
}