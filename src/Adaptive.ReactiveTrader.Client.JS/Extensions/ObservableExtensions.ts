declare module Rx {
    // extend the interface (this is how one can implement extension methods in TypeScript)
    interface ConnectableObservable<T> extends Observable<T> {
        lazyConnect(futureDisposable: SingleAssignmentDisposable): Observable<T>;
    }
}

Rx.ConnectableObservable.prototype.lazyConnect = function<T>(futureDisposable: Rx.SingleAssignmentDisposable): Rx.Observable<T> {
    var connected = false;
    return Rx.Observable.create<T>(observer=> {
        var subscription = this.subscribe(observer);
        console.info("Boom");
        if (!connected) {
            connected = true;
            if (!futureDisposable.isDisposed) {
                futureDisposable.setDisposable(this.connect());
            }
        }
        return subscription;
    }).asObservable();
};
    