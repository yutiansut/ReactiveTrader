using Foundation;
using UIKit;

namespace Adaptive.ReactiveTrader.Client.iOSTab.WatchKitExtension
{
    public static class PriceExtentions
    {
        public static NSAttributedString ToAttributedString(this UI.SpotTiles.FormattedPrice price)
        {
            var str = new NSMutableAttributedString(price.BigFigures);
            str.Append(new NSAttributedString(price.Pips, UIFont.BoldSystemFontOfSize(1)));
            str.Append(new NSAttributedString(price.TenthOfPip));

            return str;
        }
    }
}