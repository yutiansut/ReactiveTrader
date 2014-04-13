class ReferenceDataServiceClient extends ServiceClientBase implements IReferenceDataServiceClient  {
     constructor(connectionProvider: IConnectionProvider) {
         super(connectionProvider);
     }

    getCurrencyPairUpdates() : Rx.Observable<CurrencyPairUpdateDto[]> {
        return this.requestUponConnection(connection => this.getTradesForConnection(connection), 500);
    }

    private getTradesForConnection(connection: IConnection) : Rx.Observable<CurrencyPairUpdateDto[]> {
        return Rx.Observable.create<CurrencyPairUpdateDto[]>(observer => {
            var currencyPairUpdateSubscription = connection.currencyPairUpdates.subscribe(
                currencyPairUpdate=> observer.onNext([currencyPairUpdate]));

            console.log("Sending currency pair subscription...");

            connection.referenceDataHubProxy
                .invoke("GetCurrencyPairs")
                .done(currencyPairs => {
                    observer.onNext(currencyPairs);
                    console.log("Subscribed to currency pairs and received " + currencyPairs.length +" currency pairs.");
                })
                .fail(ex => observer.onError(ex));

            return currencyPairUpdateSubscription;
        })
        .publish()
        .refCount();
    }
}

        
