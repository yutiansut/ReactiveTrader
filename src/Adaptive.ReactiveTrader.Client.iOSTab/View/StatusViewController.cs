
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices;

namespace Adaptive.ReactiveTrader.Client.iOSTab.View
{
	public partial class StatusViewController : UIViewController
	{
		private readonly IReactiveTrader _reactiveTrader;
		private readonly IConcurrencyService _concurrencyService;
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		public StatusViewController (IReactiveTrader reactiveTrader, IConcurrencyService concurrencyService) : base ("StatusViewController", null)
		{
			_reactiveTrader = reactiveTrader;
			_concurrencyService = concurrencyService;

			Title = "Status";
			TabBarItem.Image = UIImage.FromBundle ("adaptive");

		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_reactiveTrader.ConnectionStatusStream
				.SubscribeOn (_concurrencyService.TaskPool)
				.ObserveOn (_concurrencyService.Dispatcher)
				.Subscribe (OnStatusChange);

			Observable.Timer (TimeSpan.FromSeconds(1), _concurrencyService.TaskPool)
				.ObserveOn (_concurrencyService.Dispatcher)
				.Subscribe(_ => OnTimer());

			// Perform any additional setup after loading the view, typically from a nib.
		}

		private void OnStatusChange(ConnectionInfo connectionInfo) {
			this.ConnectionUrl.Text = connectionInfo.Server;
			this.ConnectionStatus.Text = connectionInfo.ConnectionStatus.ToString ();
		}

		private void OnTimer() {
			var statistics = _reactiveTrader.PriceLatencyRecorder.CalculateAndReset ();

			this.ServerUpdateRate.Text = statistics.ReceivedCount.ToString ();
			this.UIUpdateRate.Text = statistics.RenderedCount.ToString ();
			this.UILatency.Text = statistics.RenderedCount.ToString ();
		}
	}
}

