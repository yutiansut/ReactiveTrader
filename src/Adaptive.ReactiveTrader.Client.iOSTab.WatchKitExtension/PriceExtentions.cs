using Foundation;
using UIKit;

namespace Adaptive.ReactiveTrader.Client.iOSTab.WatchKitExtension
{
    public static class PriceExtentions
    {
        public static NSAttributedString ToAttributedString(this UI.SpotTiles.FormattedPrice price)
        {
            var minorAttributes = new UIStringAttributes { ForegroundColor = UIColor.FromRGB(.7f, .7f, .7f), Font = UIFont.SystemFontOfSize(16), BaselineOffset = 1.5f };
            var majorAttributes = new UIStringAttributes { Font = UIFont.SystemFontOfSize(23) };

            var str = new NSMutableAttributedString(price.BigFigures, minorAttributes);
            str.Append(new NSAttributedString(price.Pips, majorAttributes));
            str.Append(new NSAttributedString(price.TenthOfPip, minorAttributes));

            return str;
        }

        public static string ToNormalString(this UI.SpotTiles.FormattedPrice price)
        {
            return $"{price.BigFigures}{price.Pips}{price.TenthOfPip}";
        }


    }
}