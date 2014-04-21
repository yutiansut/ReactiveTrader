class PriceRepository implements IPriceRepository {  
    private _pricingServiceClient: IPricingServiceClient ;
    private _priceFactory: IPriceFactory;
    
    constructor(
        pricingServiceClient: IPricingServiceClient,
        priceFactory: IPriceFactory) {
        this._priceFactory = priceFactory;
        this._pricingServiceClient = pricingServiceClient;
    }
    
    getPriceStream(currencyPair: ICurrencyPair): Rx.Observable<IPrice> {
        return Rx.Observable.defer(()=> this._pricingServiceClient.getSpotStream(currencyPair.symbol))
            .select(p=> this._priceFactory.create(p, currencyPair))
            .catch(ex => {
                console.error("Error thrown in stream " + currencyPair.symbol + ": " + ex);
                return Rx.Observable.return(new StalePrice(currencyPair));
            })
            .repeat()
            .detectStale(4000, Rx.Scheduler.timeout)
            .select(s => <IPrice>(s.isStale ? new StalePrice(currencyPair) : s.update))
            .publish()
            .refCount();
    }
} 