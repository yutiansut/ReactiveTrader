using System;
using UIKit;
using Foundation;
using Adaptive.ReactiveTrader.Client.Domain.Models.Execution;
using Adaptive.ReactiveTrader.Client.Domain.Models;

namespace Adaptive.ReactiveTrader.Client.iOS.Shared
{
    public static class TradeToStringExtention
    {
        static readonly UIStringAttributes _grey = new UIStringAttributes { ForegroundColor = UIColor.LightGray };
        static readonly UIStringAttributes _normal = new UIStringAttributes();

        static readonly UIStringAttributes _smallGrey = new UIStringAttributes { ForegroundColor = UIColor.LightGray, Font = UIFont.SystemFontOfSize(10) };
        static readonly UIStringAttributes _small = new UIStringAttributes { Font = UIFont.SystemFontOfSize(10) };

        public static NSAttributedString ToAttributedString(this ITrade trade)
        {
            var currency = trade.CurrencyPair.Replace(trade.DealtCurrency, ""); // Hack

            var sold = trade.Direction == Direction.BUY ? "Bought" : "Sold";

            var text = new NSMutableAttributedString();

            text.Append($"{sold} ", _grey);
            text.Append($"{trade.DealtCurrency} {trade.Notional:n0}", _normal);
            text.Append("\n vs ", _grey);
            text.Append(currency, _normal);
            text.Append("\n at ", _grey);
            text.Append(trade.SpotRate.ToString(), _normal);
            text.Append("\n", _normal);
            text.Append("\nTrade ID: ", _smallGrey);
            text.Append(trade.TradeId.ToString(), _small);
            return text;
        }

        public static NSAttributedString ToAttributedStringLine1(this ITrade trade)
        {
            var sold = trade.Direction == Direction.BUY ? "Bought" : "Sold";
            var text = new NSMutableAttributedString();

            text.Append($"{sold} ", _grey);
            text.Append($"{trade.DealtCurrency} {trade.Notional:n0}\n", _normal);
            return text;
        }

        public static NSAttributedString ToAttributedStringLine2(this ITrade trade)
        {
            var currency = trade.CurrencyPair.Replace(trade.DealtCurrency, ""); // Hack
            var text = new NSMutableAttributedString();

            text.Append("vs ", _grey);
            text.Append(currency, _normal);
            text.Append(" at ", _grey);
            text.Append(trade.SpotRate.ToString(), _normal);
            return text;
        }
    }

    static class NSMutableAttributedStringExtention
    {
        public static void Append(this NSMutableAttributedString attributedString, string str, UIStringAttributes attributes)
        {
            attributedString.Append(new NSAttributedString(str, attributes));
        }
    }
}

