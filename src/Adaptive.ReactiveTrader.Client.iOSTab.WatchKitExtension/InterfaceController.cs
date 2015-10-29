using System;
using Foundation;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Domain.Transport;
using System.Reactive.Concurrency;
using WatchKit;
using Adaptive.ReactiveTrader.Client.Domain.Models.ReferenceData;
using System.Collections.Generic;

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

        static string RowType = "default";

        void Setup()
        {
            StatusLabel.SetText("Connecting...");

            _reactiveTrader = new Adaptive.ReactiveTrader.Client.Domain.ReactiveTrader();
            _reactiveTrader.Initialize (UserModel.Instance.TraderId, new [] { "https://reactivetrader.azurewebsites.net/signalr" });  
            _reactiveTrader.ConnectionStatusStream
                .Where(ci => ci.ConnectionStatus == ConnectionStatus.Connected)
                //.Timeout(TimeSpan.FromSeconds (15))
                .ObserveOn(new EventLoopScheduler())
                .Subscribe(
                    _ => StatusLabel.SetText("Connected"), // _startUpViewController.PresentViewController(tabBarController, true, null),
                    ex =>  

                        StatusLabel.SetText("Failed: " + ex) // _startUpViewController.DisplayMessages (false, "Disconnected", "Unable to connect")
                );

//            onCurrencyPair
//                .Where(update => update.UpdateType == UpdateType.Add)
//                .ObserveOn(new EventLoopScheduler())
//                .Select(u => u.CurrencyPair)
//                .Buffer(TimeSpan.FromSeconds(1))
//                .Where(updates => updates.Any())
//                .Subscribe(update =>
//                {
//                        _pairs.AddRange(update);
//                        UpdateSubscription();
//                        UpdateTable();
//                });
//3

            Table.SetNumberOfRows(1, RowType);
            _mergedStream = Observable.Interval(TimeSpan.FromSeconds(.1))

                .Subscribe(

                    _ =>
                    {
                        for (int i = 0; i < Table.NumberOfRows; i++)
                        {
                            Console.WriteLine(i);
                            var rowController = (RowController)Table.GetRowController(i);
                            rowController.UpdatePrice(null);
                        }
                    }

                );
//            
//
//                .Subscribe(updates => 
//                
//                    {
//                        foreach (var update in updates)
//                        {
//                            
//                            Console.WriteLine("Update: " +update.UpdateType);
//                        }
//                    });
        }

        IDisposable _mergedStream;

        void UpdateSubscription()
        {
            

//            int i = 0;

//            _mergedStream = _pairs
//                .Select(pair => pair.PriceStream)
//                .Merge()
//                .Buffer(TimeSpan.FromSeconds(1))
//                .Do(_ => Console.WriteLine("update"))
//                .Subscribe(prices =>
//                
//                    {
//                    
//                        IEnumerable<IGrouping<ICurrencyPair, IPrice>> grouped = prices.GroupBy(p => p.CurrencyPair);
//                    
//                        foreach (var g in grouped)
//                        {
//                            var rowIndex = _pairs.IndexOf(g.Key);
//                            var rowController = (RowController)Table.GetRowController(rowIndex);
//
//
//                            rowController.UpdatePrice(g.Last());
////                            g.Last().
//                        }
//
//                    });
//

//            if (_mergedStream != null)
//            {
//                _mergedStream.Dispose();
//            }
        }

        void UpdateTable()
        {
            Table.SetNumberOfRows(_pairs.Count, RowType);
//            int i = 0;
//
//            foreach (var pair in _pairs)
//            {
//                var rowController = (RowController)Table.GetRowController(i);
//                rowController.CurrencyPair = pair;
//                i++;
//            }
        }

        List<ICurrencyPair> _pairs = new List<ICurrencyPair>();

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();



        }

        public override void DidDeactivate()
        {
            // This method is called when the watch view controller is no longer visible to the user.
            Console.WriteLine("{0} did deactivate", this);

        }
    }


}

