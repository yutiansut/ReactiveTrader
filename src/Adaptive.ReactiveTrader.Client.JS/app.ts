 $(document).ready(()=> {
    var reactiveTrader = <IReactiveTrader> new ReactiveTrader();

    reactiveTrader.initialize("olivier", ["http://localhost:800"]);
    reactiveTrader.connectionStatusStream.subscribe(s=> console.log("Connection status: " + s));

    var priceLatencyRecorder = new PriceLatencyRecorder();
    var pricingViewModelFactory = new PricingViewModelFactory(priceLatencyRecorder);
    var spotTilesViewModel = new SpotTilesViewModel(reactiveTrader.referenceDataRepository, pricingViewModelFactory);
    var blotterViewModel = new BlotterViewModel(reactiveTrader.tradeRepository);
    var connectivityStatusViewModel = new ConnectivityStatusViewModel(reactiveTrader, priceLatencyRecorder);
    var shellViewModel = new ShellViewModel(spotTilesViewModel, blotterViewModel, connectivityStatusViewModel);

    ko.applyBindings(shellViewModel);
});
