using System;
using Foundation;
using WatchKit;

namespace Adaptive.ReactiveTrader.Client.iOSTab.WatchKitExtension
{
    public partial class RowController : WKInterfaceController
    {
        public RowController ()
        {
        }

        
        public WKInterfaceLabel TestLabel
        {
            get
            {
                return this.Label;
            }

        }
    }
}

