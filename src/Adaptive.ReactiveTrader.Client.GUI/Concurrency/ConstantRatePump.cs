using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Adaptive.ReactiveTrader.Client.Concurrency
{
    class ConstantRatePump : IConstantRatePump
    {
        private readonly IObservable<Unit> _tick;
 
        public ConstantRatePump(IConcurrencyService concurrencyService)
        {
            _tick = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(125), concurrencyService.Dispatcher)
                              .Select(_ => Unit.Default)   // Use underscore (_) as a parameter name to indicate that it is ignored/not used
                              .Publish()
                              .RefCount();
        }
 
        public IObservable<Unit> Tick
        {
            get { return _tick; }
        }
    }
}