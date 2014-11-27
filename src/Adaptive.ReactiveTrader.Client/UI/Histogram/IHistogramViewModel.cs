using Adaptive.ReactiveTrader.Client.Domain.Instrumentation;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;

namespace Adaptive.ReactiveTrader.Client.UI.Histogram
{
    public interface IHistogramViewModel
    {
        void OnStatistics(Statistics statistics, SpotTileSubscriptionMode subscriptionMode);
        string ImageSource { get; }
    }
}