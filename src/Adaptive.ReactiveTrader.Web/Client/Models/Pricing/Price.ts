class Price implements IPrice, IPriceLatency {
    constructor(
        bid: ExecutablePrice,
        ask: ExecutablePrice,
        valueDate: Date,
        currencyPair: ICurrencyPair) {

        this.bid = bid;
        this.ask = ask;
        this.valueDate = valueDate;
        this.currencyPair = currencyPair;
        this.isStale = false;

        bid.parent = this;
        ask.parent = this;

        this.spread = (ask.rate - bid.rate) * Math.pow(10, currencyPair.pipsPosition);
    }

    bid: IExecutablePrice;
    ask: IExecutablePrice;
    currencyPair: ICurrencyPair;
    valueDate: Date;
    spread: number;
    isStale: boolean; 

    get mid(): number {
        return (this.bid.rate + this.ask.rate) / 2;
    }

    // IPriceLatency implementation

    private _receivedTimestamp: number;
    private _renderTimestamp: number;

    get uiProcessingTimeMs() {
        return this._renderTimestamp - this._receivedTimestamp;
    }

    displayedOnUi(): void {
        this._renderTimestamp = performance.now();
    }

    receivedInGuiProcess(): void {
        this._receivedTimestamp = performance.now();
    }
} 