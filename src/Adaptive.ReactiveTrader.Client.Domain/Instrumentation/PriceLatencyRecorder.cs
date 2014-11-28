using System.IO;
using Adaptive.ReactiveTrader.Client.Domain.Models.Pricing;
using HdrHistogram.NET;

namespace Adaptive.ReactiveTrader.Client.Domain.Instrumentation
{
    public class PriceLatencyRecorder : IPriceLatencyRecorder
    {
        private readonly Histogram _combinedLatency;
        private readonly object _histogramLock = new object();

        public PriceLatencyRecorder()
        {
            _combinedLatency = GetHistogram();
        }

        public void OnRendered(IPrice price)
        {
            var priceLatency = price as IPriceLatency;
            if (priceLatency != null)
            {
                priceLatency.DisplayedOnUi();

                long totalLatencyTicks = priceLatency.TotalLatencyTicks;
                if (totalLatencyTicks > 0)
                    _combinedLatency.recordValue(totalLatencyTicks);
            }
        }

        public void OnReceived(IPrice price)
        {
            var priceLatency = price as IPriceLatency;
            if (priceLatency != null)
            {
                priceLatency.ReceivedInGuiProcess();
            }
        }

        public Statistics CalculateAndReset()
        {
            var stats = new Statistics();

            lock (_histogramLock)
            {
                stats.TotalLatencyMax = _combinedLatency.getMaxValue();
                var sw = new StringWriter();
                _combinedLatency.outputPercentileDistribution(sw);
                stats.Histogram = sw.ToString();

                _combinedLatency.reset();
            }

            return stats;
        }

        private Histogram GetHistogram()
        {
            return new Histogram((long)(1000000 * 60 * 30), 3); // 1 ns to 30 minutes
        }
    }
}