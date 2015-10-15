using System;
using Foundation;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Domain.Transport;
using System.Reactive.Concurrency;
using WatchKit;
using ObjCRuntime;

namespace Adaptive.ReactiveTrader.Client.iOSTab.WatchKitExtension
{
    public partial class InterfaceController : WKInterfaceController
    {
        Adaptive.ReactiveTrader.Client.Domain.ReactiveTrader _reactiveTrader;

        public InterfaceController(IntPtr handle)
            : base(handle)
        {
        }

        public override void Awake(NSObject context)
        {
            base.Awake(context);
                
            // Configure interface objects here.
            Console.WriteLine("{0} awake with context", this);
        }

        public override void WillActivate()
        {
            // This method is called when the watch view controller is about to be visible to the user.
            Console.WriteLine("{0} will activate", this);
            Setup();

        }

        void Setup()
        {
            StatusLabel.SetText("Connecting...");

            _reactiveTrader = new Adaptive.ReactiveTrader.Client.Domain.ReactiveTrader();
            _reactiveTrader.Initialize (UserModel.Instance.TraderId, new [] { "https://reactivetrader.azurewebsites.net/signalr" });  
            _reactiveTrader.ConnectionStatusStream
                .Where(ci => ci.ConnectionStatus == ConnectionStatus.Connected)
                .Timeout(TimeSpan.FromSeconds (15))
                .ObserveOn(new EventLoopScheduler())
                .Subscribe(
                    _ => StatusLabel.SetText("Connected."), // _startUpViewController.PresentViewController(tabBarController, true, null),
                    ex =>  StatusLabel.SetText("Failed") // _startUpViewController.DisplayMessages (false, "Disconnected", "Unable to connect")
                );


            _reactiveTrader.ReferenceData
                .GetCurrencyPairsStream ()
                .ObserveOn(new EventLoopScheduler())
                .Subscribe(updates => 
                
                    {
                        foreach (var update in updates)
                        {
                            Console.WriteLine("Update: " +update.UpdateType);
                        }
                    });
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();




            Table.SetNumberOfRows(1, "default");
            var rowController = (RowController)Table.GetRowController(0);
            rowController.TestLabel.SetText("hello world");
        }

        public override void DidDeactivate()
        {
            // This method is called when the watch view controller is no longer visible to the user.
            Console.WriteLine("{0} did deactivate", this);

        }
    }


}

