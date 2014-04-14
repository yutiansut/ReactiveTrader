class PricingViewModelFactory implements IPricingViewModelFactory {
    private _priceLatencyRecorder: IPriceLatencyRecorder;
    
    constructor(priceLatencyRecorder: IPriceLatencyRecorder) {
        this._priceLatencyRecorder = priceLatencyRecorder;
    }

    create(currencyPair: ICurrencyPair): IPricingViewModel {
        return new PricingViewModel(currencyPair, this._priceLatencyRecorder);
    }
}