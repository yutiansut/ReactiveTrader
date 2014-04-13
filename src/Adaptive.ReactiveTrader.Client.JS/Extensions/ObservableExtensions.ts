declare module Rx {
    // extend the interface (this is how one can implement extension methods in TypeScript)
    interface ConnectableObservable<T> extends Observable<T> {
        lazyConnect(futureDisposable: SingleAssignmentDisposable): Observable<T>;
    }

    interface Observable<T> {
        detectStale(stalenessPeriodMs: number, scheduler: IScheduler): Observable<IStale<T>>;
        cacheFirstResult(): Observable<T>;
    }
}

Rx.Observable.prototype.detectStale = function <T>(stalenessPeriodMs: number, scheduler: Rx.IScheduler): Rx.Observable<IStale<T>> {
    return Rx.Observable.create<IStale<T>>(observer => {
        var timerSubscription = new Rx.SerialDisposable();

        var scheduleStale = () => {
            timerSubscription.setDisposable(Rx.Observable
                .timer(stalenessPeriodMs, scheduler)
                .subscribe(
                _ => {
                    observer.onNext(new Stale<T>(true, null));
                }));
        };

        var sourceSubscription = this.subscribe(
            x => {
                // cancel any scheduled stale update
                timerSubscription.getDisposable().dispose();

                observer.onNext(new Stale<T>(false, x));

                scheduleStale();
            },
            ex => observer.onError(ex),
            () => observer.onCompleted());

        scheduleStale();

        return new Rx.CompositeDisposable(sourceSubscription, timerSubscription);
    });
};

Rx.Observable.prototype.cacheFirstResult = function<T>(): Rx.Observable<T> {
    return this.take(1).publishLast().lazyConnect(new Rx.SingleAssignmentDisposable());
};

Rx.ConnectableObservable.prototype.lazyConnect = function<T>(futureDisposable: Rx.SingleAssignmentDisposable): Rx.Observable<T> {
    var connected = false;
    return Rx.Observable.create<T>(observer=> {
        var subscription = this.subscribe(observer);
        if (!connected) {
            connected = true;
            if (!futureDisposable.isDisposed) {
                futureDisposable.setDisposable(this.connect());
            }
        }
        return subscription;
    }).asObservable();
};
