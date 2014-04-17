 $(document).ready(()=> {
    var reactiveTrader = <IReactiveTrader> new ReactiveTrader();

    reactiveTrader.initialize("Mike Trader", [""]);

    // uncomment the following line to test with reactive trader Server GUI

    //reactiveTrader.initialize("Mike Trader", ["http://localhost:8080"]);

     reactiveTrader.connectionStatusStream.subscribe(s=> console.log("Connection status: " + s));

    var pricingViewModelFactory = new PricingViewModelFactory(reactiveTrader.priceLatencyRecorder);
    var spotTilesViewModel = new SpotTilesViewModel(reactiveTrader.referenceDataRepository, pricingViewModelFactory);
    var blotterViewModel = new BlotterViewModel(reactiveTrader.tradeRepository);
    var connectivityStatusViewModel = new ConnectivityStatusViewModel(reactiveTrader, reactiveTrader.priceLatencyRecorder);
    var shellViewModel = new ShellViewModel(spotTilesViewModel, blotterViewModel, connectivityStatusViewModel);

    ko.applyBindings(shellViewModel);
});
