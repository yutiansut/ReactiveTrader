using System;
using Adaptive.ReactiveTrader.Client.Domain;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Collections.Generic;
using Adaptive.ReactiveTrader.Client.Domain.Models.Execution;
using UIKit;
using Adaptive.ReactiveTrader.Client.Concurrency;
using Adaptive.ReactiveTrader.Client.Domain.Models;
using Foundation;
using Adaptive.ReactiveTrader.Client.iOS.Shared;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
    public class NotificationGenerator
    {
        readonly IReactiveTrader _reactiveTrader;
        readonly IConcurrencyService _concurrencyService;
        readonly CompositeDisposable _disposables = new CompositeDisposable();

        public NotificationGenerator(IReactiveTrader reactiveTrader, IConcurrencyService concurrencyService)
        {
            _reactiveTrader = reactiveTrader;
            _concurrencyService = concurrencyService;
        }

        public void Initialise() {
            _disposables.Add(
                _reactiveTrader.TradeRepository
                .GetTradesStream()
                .SubscribeOn(_concurrencyService.TaskPool)
                .ObserveOn(_concurrencyService.Dispatcher)
                .Skip(2) // Skip out past trades on start up
                .Subscribe(OnTradeUpdates)
            );
        }

        public void Dispose ()
        {
            _disposables.Dispose();
        }

        public static void RegisterNotifications()
        {
            var action = new UIMutableUserNotificationAction
                {
                    Title = "Trade",
                    Identifier = "trade",
                    ActivationMode = UIUserNotificationActivationMode.Foreground,
                    AuthenticationRequired = false
                };

            var actionCategory = new UIMutableUserNotificationCategory { Identifier = "trade" };

            var categories = new NSMutableSet();
            categories.Add(actionCategory);

            actionCategory.SetActions(new UIUserNotificationAction[] { action }, UIUserNotificationActionContext.Default);

            var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Sound, categories);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
        }

        void OnTradeUpdates(IEnumerable<ITrade> trades)
        {                
           
            foreach (var trade in trades)
            {
                var boughtOrSold = trade.Direction == Direction.BUY ? "bought" : "sold";

                var currencyOne = trade.CurrencyPair.Substring(0, 3);
                var currencyTwo = trade.CurrencyPair.Substring(3, 3);

                var userInfo = new NSMutableDictionary
                {
                    { (NSString)"trade", trade.ToNSString() },
                    { (NSString)"baseCurrency", (NSString)currencyOne },
                    { (NSString)"counterCurrency", (NSString)currencyTwo }
                };

                var notification = new UILocalNotification
                {
                    AlertBody = $"'{trade.TraderName}' {boughtOrSold} {trade.Notional:n0} {trade.DealtCurrency} vs {currencyTwo} at {trade.SpotRate}",
                    Category = "trade",
                    UserInfo = userInfo,
                    AlertTitle = "Trade Executed",
                    AlertAction = $"Show {currencyOne} / {currencyTwo}",
                    HasAction = true,
                    SoundName = UILocalNotification.DefaultSoundName
                };
                              
                Console.WriteLine("Notification: " + notification.AlertBody);                 
                UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
            }
        }
    }
}

