using System.Reactive.Concurrency;
using Adaptive.ReactiveTrader.Client.Concurrency;

namespace Adaptive.ReactiveTrader.Client.Android.Concurrency
{
    public sealed class ConcurrencyService : IConcurrencyService
    {
        public IScheduler Dispatcher { get; set; }

        public IScheduler TaskPool
        {
            get { return TaskPoolScheduler.Default; }
        }
    }
}