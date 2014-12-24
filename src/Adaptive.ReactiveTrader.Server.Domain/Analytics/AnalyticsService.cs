using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Adaptive.ReactiveTrader.Shared.DTO.Analytics;
using Adaptive.ReactiveTrader.Shared.DTO.Execution;
using Adaptive.ReactiveTrader.Shared.DTO.Pricing;

namespace Adaptive.ReactiveTrader.Server.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsPublisher _analyticsPublisher;
        private readonly IDictionary<string, PriceDto> _priceCache = new Dictionary<string, PriceDto>();
        private readonly IDictionary<string, CurrencyPairTracker> _ccyPairTracker = new Dictionary<string, CurrencyPairTracker>();
        private readonly EventLoopScheduler _eventLoopScheduler = new EventLoopScheduler();

        private PositionUpdatesDto _currentPositionUpdatesDto = new PositionUpdatesDto();
        
        public AnalyticsService(IAnalyticsPublisher analyticsPublisher)
        {
            _analyticsPublisher = analyticsPublisher;
            _eventLoopScheduler.SchedulePeriodic(TimeSpan.FromSeconds(10), PublishPositionReport);
        }
        public void OnTrade(TradeDto trade)
        {
            _eventLoopScheduler.Schedule(() =>
            {
                CurrencyPairTracker currencyPairTracker;
                if (!_ccyPairTracker.TryGetValue(trade.CurrencyPair, out currencyPairTracker))
                {
                    currencyPairTracker = new CurrencyPairTracker(trade.CurrencyPair);
                }
                currencyPairTracker.OnTrade(trade, _priceCache);
                PublishPositionReport();
            });
        }

        public void OnPrice(PriceDto priceDto)
        {
            _eventLoopScheduler.Schedule(() => _priceCache[priceDto.Symbol] = priceDto);
        }

        private void PublishPositionReport()
        {
            var pud = new PositionUpdatesDto();
            
            pud.CurrentPositions = _ccyPairTracker
                .Values
                .Where(ccp => ccp.TradeCount > 0)
                .Select(ccp => new CurrencyPairPositionDto()
                {
                    Symbol = ccp.CurrencyPair,
                    BasePnl = ccp.CurrentPosition.BasePnl,
                    BaseTradedAmount = ccp.CurrentPosition.BaseTradedAmount
                })
                .ToArray();

            var usdPnl = _ccyPairTracker.Values
                         .Where(ccp => ccp.TradeCount > 0)
                         .Sum(ccp => ccp.CurrentPosition.UsdPnl);


            var now = DateTimeOffset.UtcNow;
            var window = now.AddMinutes(-15);

            pud.History = _currentPositionUpdatesDto.History
                                    .Where(hpu => hpu.Timestamp >= window)
                                    .Concat(new [] { new HistoricPositionDto() {Timestamp = now, UsdPnl = usdPnl}})
                                    .ToArray();

            _currentPositionUpdatesDto = pud;

            // todo need to do something different here, I think.
            _analyticsPublisher.Publish(_currentPositionUpdatesDto).Wait(TimeSpan.FromSeconds(10));
        }
    }
}