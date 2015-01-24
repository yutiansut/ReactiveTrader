using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Android.Content;
using Android.OS;

namespace Adaptive.ReactiveTrader.Client.Android.Concurrency
{
    public class AndroidUiScheduler : IScheduler
    {
        private readonly Handler _handler;

        public AndroidUiScheduler(Context context)
        {
            _handler = new Handler(context.MainLooper);
        }

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            _handler.Post(() =>
            {
                action(this, state);
            });

            return Disposable.Empty;    // xamtodo
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            _handler.PostDelayed(() =>
            {
                action(this, state);
            }, (long)dueTime.TotalMilliseconds);

            return Disposable.Empty;
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            var due = (TimeSpan)(dueTime - DateTimeOffset.Now);
            if (due.TotalMilliseconds < 0)
            {
                due = TimeSpan.Zero;
            }

            _handler.PostDelayed(() =>
            {
                action(this, state);
            }, (long)due.TotalMilliseconds);

            return Disposable.Empty;
        }

        public DateTimeOffset Now
        {
            get { return DateTimeOffset.Now; }
        }
    }
}