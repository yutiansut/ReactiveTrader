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
            _recyclerView.ViewTreeObserver.RemoveGlobalOnLayoutListener(this);
            var viewWidth = _recyclerView.MeasuredWidth;
          //  const double tileWidth = 331;
            //int newSpanCount = (int)Math.Floor(viewWidth / tileWidth);
            _gridLayoutManager.SpanCount = 4;
            _gridLayoutManager.RequestLayout();
        }
    }
}