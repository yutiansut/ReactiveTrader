class PriceFactory implements IPriceFactory {
    private _executionRepository: IExecutionRepository;
    private _priceLatencyRecored: IPriceLatencyRecorder;

    constructor(executionRepository: IExecutionRepository, priceLatencyRecored: IPriceLatencyRecorder) {
        this._executionRepository = executionRepository;
        this._priceLatencyRecored = priceLatencyRecored;
    }

    create(priceDto: PriceDto, currencyPair: ICurrencyPair) {
        var bid = new ExecutablePrice(Direction.Sell, priceDto.b, this._executionRepository);
        var ask = new ExecutablePrice(Direction.Buy, priceDto.a, this._executionRepository);
        var valueDate = new Date(priceDto.d);
        var price = new Price(bid, ask, valueDate, currencyPair);

        this._priceLatencyRecored.onReceived(price);

        return price;
    }
} 

