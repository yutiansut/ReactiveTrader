using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Adaptive.ReactiveTrader.Client.Concurrency
{
    public sealed class ConcurrencyService : IConcurrencyService
    {
        private readonly CoreDispatcherScheduler _dispatcherScheduler;
        private readonly PeriodicBatchScheduler _scheduler;

        public ConcurrencyService()
        {
            _dispatcherScheduler = new CoreDispatcherScheduler(CoreApplication.MainView.CoreWindow.Dispatcher);
            _scheduler = new PeriodicBatchScheduler(_dispatcherScheduler);
        }
        public IScheduler Dispatcher
        {
            get { return _dispatcherScheduler; }
        }

        public IScheduler ThreadPool
        {
            get { return TaskPoolScheduler.Default; }
        }

        public IScheduler DispatcherPeriodic
        {
            get { return _scheduler; }
        }
    }
}
