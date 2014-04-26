$(document).ready(() => {
    // 5 minutes session, we disconnect users so they don't eat up too many websocket connections on Azure for too long
    var sessionExpirationSeconds = 5 * 60;

    // generate random username
    var username = "Web-" + Math.floor((Math.random() * 1000) + 1);

    var reactiveTrader = <IReactiveTrader> new ReactiveTrader();
    var servers = location.search.indexOf("?server=local") == -1 ? [""] : ["http://localhost:8080"];
    reactiveTrader.initialize(username, servers);

    var pricingViewModelFactory = new PricingViewModelFactory(reactiveTrader.priceLatencyRecorder);
    var spotTilesViewModel = new SpotTilesViewModel(reactiveTrader.referenceDataRepository, pricingViewModelFactory);
    var blotterViewModel = new BlotterViewModel(reactiveTrader.tradeRepository);
    var connectivityStatusViewModel = new ConnectivityStatusViewModel(reactiveTrader, reactiveTrader.priceLatencyRecorder);
    var sessionExpirationService = new SessionExpirationService(sessionExpirationSeconds);
    var shellViewModel = new ShellViewModel(spotTilesViewModel, blotterViewModel, connectivityStatusViewModel, sessionExpirationService, reactiveTrader);

    // TODO this should be moved somewhere else
    this.fadeTrade = (element, index, data) => {
        $(element)
            .animate({ backgroundColor: '#7A9EFF' }, 200)
            .animate({ backgroundColor: 'white' }, 800);
    };

    ko.applyBindings(shellViewModel);
});
