using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Domain.Models.Pricing;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using Adaptive.ReactiveTrader.Client.Domain.ServiceClients;
using Adaptive.ReactiveTrader.Shared.Extensions;
using log4net;

namespace Adaptive.ReactiveTrader.Client.Domain.Repositories
{
    class PriceRepository : IPriceRepository
    {
        private readonly IPricingServiceClient _pricingServiceClient;
        private readonly IPriceFactory _priceFactory;
        private static readonly ILog Log = LogManager.GetLogger(typeof(PriceRepository));

        public PriceRepository(IPricingServiceClient pricingServiceClient, IPriceFactory priceFactory)
        {
            _pricingServiceClient = pricingServiceClient;
            _priceFactory = priceFactory;
        }

        public IObservable<IPrice> GetPriceStream(ICurrencyPair currencyPair)
        {
            return Observable.Defer(() => _pricingServiceClient.GetSpotStream(currencyPair.Symbol))
                .Select(p => _priceFactory.Create(p, currencyPair))
                .Catch<IPrice, Exception>(ex =>
                {
                    Log.Error("Error thrown in stream " + currencyPair.Symbol, ex);
                    // if the stream errors (server disconnected), we push a stale price 
                    return Observable
                            .Return<IPrice>(new StalePrice(currencyPair))
                            // terminate the observable in 3sec so the repeat does not kick-off immediatly
                            .Concat(Observable.Timer(TimeSpan.FromSeconds(3)).IgnoreElements().Select(_ => new StalePrice(currencyPair)));
                }) 
                .Repeat()                                               // and resubscribe
                .DetectStale(TimeSpan.FromSeconds(4),  Scheduler.Default)
                .Select(s => s.IsStale ? new StalePrice(currencyPair) : s.Update)
                .Publish()
                .RefCount();
        }
    }
}