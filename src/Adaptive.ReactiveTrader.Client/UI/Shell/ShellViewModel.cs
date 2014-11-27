using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Concurrency;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.Domain.Instrumentation;
using Adaptive.ReactiveTrader.Client.UI.Blotter;
using Adaptive.ReactiveTrader.Client.UI.Connectivity;
using Adaptive.ReactiveTrader.Client.UI.Histogram;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Shared.Extensions;
using Adaptive.ReactiveTrader.Shared.UI;
using PropertyChanged;

namespace Adaptive.ReactiveTrader.Client.UI.Shell
{
    [ImplementPropertyChanged]
    public class ShellViewModel : ViewModelBase, IShellViewModel
    {
        private readonly SerialDisposable _warmupReset = new SerialDisposable();
        private readonly IConcurrencyService _concurrencyService;
        private readonly IPriceLatencyRecorder _priceLatencyRecorder;
        public ISpotTilesViewModel SpotTiles { get; private set; }
        public IBlotterViewModel Blotter { get; private set; }
        public IConnectivityStatusViewModel ConnectivityStatus { get; private set; }
        public IHistogramViewModel Histogram { get; private set; }

        private static readonly TimeSpan StatsFrequency = TimeSpan.FromSeconds(5);
        

        public ShellViewModel(ISpotTilesViewModel spotTiles, 
            IBlotterViewModel blotter, 
            IConnectivityStatusViewModel connectivityStatusViewModel, 
            IHistogramViewModel histogramViewModel,
            IReactiveTrader reactiveTrader,
            IConcurrencyService concurrencyService)
        {
            _concurrencyService = concurrencyService;
            _priceLatencyRecorder = reactiveTrader.PriceLatencyRecorder;
            
            SpotTiles = spotTiles;
            Blotter = blotter;
            ConnectivityStatus = connectivityStatusViewModel;
            Histogram = histogramViewModel;

            SpotTiles.ObserveProperty(st => st.SubscriptionMode)
                .Scan(Tuple.Create(SpotTileSubscriptionMode.OnDispatcher, SpotTileSubscriptionMode.OnDispatcher), (agg, next) => Tuple.Create(agg.Item2, next))
                .Subscribe(OnSubscriptionModeChange);
        }
        private void OnSubscriptionModeChange(Tuple<SpotTileSubscriptionMode, SpotTileSubscriptionMode> prevNextSubscriptionModes)
        {
            ResetStats(prevNextSubscriptionModes.Item1);

            _warmupReset.Disposable = _concurrencyService.Dispatcher.Schedule(TimeSpan.FromSeconds(5), () => ResetStats(prevNextSubscriptionModes.Item2));
        }

        private void ResetStats(SpotTileSubscriptionMode spotTileSubscriptionMode)
        {
            var stats = _priceLatencyRecorder.CalculateAndReset();

            if (stats == null)
                return;

            ConnectivityStatus.OnStatistics(stats, StatsFrequency);
            Histogram.OnStatistics(stats, spotTileSubscriptionMode);

        }
    }
}
