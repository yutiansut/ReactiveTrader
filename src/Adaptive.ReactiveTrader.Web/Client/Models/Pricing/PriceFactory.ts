class PriceFactory implements IPriceFactory {
    private _executionRepository: IExecutionRepository;
    private _priceLatencyRecored: IPriceLatencyRecorder;

    constructor(executionRepository: IExecutionRepository, priceLatencyRecored: IPriceLatencyRecorder) {
        this._executionRepository = executionRepository;
        this._priceLatencyRecored = priceLatencyRecored;
    }

    create(priceDto: PriceDto, currencyPair: ICurrencyPair) {
        var bid = new ExecutablePrice(Direction.Sell, priceDto.Bid, this._executionRepository);
        var ask = new ExecutablePrice(Direction.Buy, priceDto.Ask, this._executionRepository);
        var price = new Price(bid, ask, priceDto.Mid, priceDto.QuoteId, priceDto.ValueDate, currencyPair);

        this._priceLatencyRecored.onReceived(price);

        return price;
    }
} 

