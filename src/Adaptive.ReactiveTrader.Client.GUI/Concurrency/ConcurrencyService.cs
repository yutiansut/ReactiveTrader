using System.Net.Mime;
using System.Reactive.Concurrency;
using System.Windows;

namespace Adaptive.ReactiveTrader.Client.Concurrency
{
    public sealed class ConcurrencyService : IConcurrencyService
    {
        private IScheduler _dispatcher;
        public IScheduler Dispatcher
        {
            get { return _dispatcher ?? (_dispatcher = new DispatcherScheduler(Application.Current.Dispatcher)); }
        }

        public IScheduler TaskPool
        {
            get { return ThreadPoolScheduler.Instance; }
        }

    }
}