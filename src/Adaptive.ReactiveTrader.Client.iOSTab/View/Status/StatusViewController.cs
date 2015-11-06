
using System;
using CoreGraphics;
using System.Collections.Generic; // For List<T>

using Foundation;
using UIKit;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using Adaptive.ReactiveTrader.Client.iOS.Shared;
using System.Reactive.Subjects;

namespace Adaptive.ReactiveTrader.Client.iOSTab.View
{
	public partial class StatusViewController : UIViewController
	{
		private readonly IReactiveTrader _reactiveTrader;
		private readonly IConcurrencyService _concurrencyService;
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		private ConnectionInfo _lastConnectionInfo;

        ISubject<bool> _notificationsEnabled;

        public StatusViewController (IReactiveTrader reactiveTrader, IConcurrencyService concurrencyService, ISubject<bool> notificationsEnabled) : base ("StatusViewController", null)
		{
            _notificationsEnabled = notificationsEnabled;
			_reactiveTrader = reactiveTrader;
			_concurrencyService = concurrencyService;

			Title = "Status";
			TabBarItem.Image = UIImage.FromBundle ("tab_status");

			_disposables.Add (
				_reactiveTrader.ConnectionStatusStream
					.SubscribeOn (_concurrencyService.TaskPool)
					.ObserveOn (_concurrencyService.Dispatcher)
					.Subscribe (OnStatusChange));

			_disposables.Add (
				Observable.Interval (TimeSpan.FromSeconds(1), _concurrencyService.TaskPool)
					.ObserveOn (_concurrencyService.Dispatcher)
					.Subscribe(_ => OnTimer()));

		}

		protected override void Dispose (bool disposing)
		{	
			base.Dispose (disposing);

			if (disposing) {
				_disposables.Dispose();
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

			// The connection detail text area wraps and is resizable.
			// Note that this requires us to turn off Auto-layout for this view.

			this.ConnectionDetail.LineBreakMode = UILineBreakMode.CharacterWrap;

			this.TraderId.Text = UserModel.Instance.TraderId;

			if (_lastConnectionInfo != null) {
				OnStatusChange (_lastConnectionInfo);
				_lastConnectionInfo = null;
			}

            _notificationsSwitch.Bind(_notificationsEnabled)
                .Add(_disposables);
		}

		private void OnStatusChange(ConnectionInfo connectionInfo)
		{
			if (this.IsViewLoaded) {
				this.ConnectionDetail.Text = connectionInfo.Server;
				this.ConnectionDetail.SizeToFit (); // Multi-line, with initially small height.
				this.ConnectionStatus.Text = connectionInfo.ConnectionStatus.ToString ();
			} else {
				_lastConnectionInfo = connectionInfo;
			}
		}

		private void OnTimer()
		{
			var statistics = _reactiveTrader.PriceLatencyRecorder.CalculateAndReset ();

			if (this.IsViewLoaded) {
				this.ServerUpdateRate.Text = string.Format ("{0} / sec", statistics.ReceivedCount);
				this.UIUpdateRate.Text = string.Format ("{0} / sec", statistics.RenderedCount);
				this.UILatency.Text = string.Format ("{0} ms", statistics.RenderedCount);
			}
		}

		partial void LinkTouchUpInside (NSObject sender)
		{
            var url = new NSUrl("http://www.weareadaptive.com");
            UIApplication.SharedApplication.OpenUrl(url);
		}
	}
}

