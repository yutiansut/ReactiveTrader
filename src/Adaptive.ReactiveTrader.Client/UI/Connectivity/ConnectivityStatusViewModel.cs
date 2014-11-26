using System;
using System.IO;
using System.Reactive.Linq;
using Adaptive.ReactiveTrader.Client.Concurrency;
using Adaptive.ReactiveTrader.Client.Domain;
using Adaptive.ReactiveTrader.Client.Domain.Instrumentation;
using Adaptive.ReactiveTrader.Client.Domain.Transport;
using Adaptive.ReactiveTrader.Shared.Logging;
using Adaptive.ReactiveTrader.Shared.UI;

namespace Adaptive.ReactiveTrader.Client.UI.Connectivity
{
    public class ConnectivityStatusViewModel : ViewModelBase, IConnectivityStatusViewModel
    {
        private readonly IProcessorMonitor _processorMonitor;
        private readonly Func<IGnuPlot> _gnuPlotFactory;
        private static readonly TimeSpan StatsFrequency = TimeSpan.FromSeconds(5);

        private readonly IPriceLatencyRecorder _priceLatencyRecorder;

        public ConnectivityStatusViewModel(
            IReactiveTrader reactiveTrader, 
            IConcurrencyService concurrencyService, 
            ILoggerFactory loggerFactory,
            IProcessorMonitor processorMonitor,
            Func<IGnuPlot> gnuPlotFactory)
        {
            _processorMonitor = processorMonitor;
            _gnuPlotFactory = gnuPlotFactory;
            _priceLatencyRecorder = reactiveTrader.PriceLatencyRecorder;
            var log = loggerFactory.Create(typeof (ConnectivityStatusViewModel));

            if (!_processorMonitor.IsAvailable)
            {
                CpuPercent = "N/A";
                CpuTime = "N/A";
            }

            reactiveTrader.ConnectionStatusStream
                .ObserveOn(concurrencyService.Dispatcher)
                .SubscribeOn(concurrencyService.TaskPool)
                .Subscribe(
                OnStatusChange,
                ex => log.Error("An error occurred within the connection status stream.", ex));

            Observable
                .Interval(StatsFrequency, concurrencyService.Dispatcher)
                .Subscribe(OnTimerTick);
        }

        private void OnTimerTick(long _)
        {
            var stats = _priceLatencyRecorder.CalculateAndReset();
            
            if (stats == null)
                return;

            UiLatency = stats.UiLatencyMax;
            UiUpdates = stats.RenderedCount;
            TicksReceived = stats.ReceivedCount;
            
            Histogram = stats.Histogram;

            if (!Disconnected && Server != null && Server.Contains("localhost"))
            {
                ServerClientLatency = stats.ServerLatencyMax + "ms";
                TotalLatency = stats.TotalLatencyMax + "ms";
            }
            else
            {
                ServerClientLatency = "N/A";
                TotalLatency = "N/A";
            }

            if (_processorMonitor.IsAvailable)
            {
                var cpuTime = _processorMonitor.CalculateProcessingAndReset();
                CpuTime = Math.Round(cpuTime.TotalMilliseconds, 0).ToString();
                CpuPercent = Math.Round(cpuTime.TotalMilliseconds / (Environment.ProcessorCount * StatsFrequency.TotalMilliseconds) * 100, 0).ToString();
            }

            // Gnu plot
            var gnuPlot = _gnuPlotFactory();
            /*
             * #plot commands
set terminal png
set output 'plot.png'
set logscale x
unset xtics
set key top left
set style line 1 lt 1 lw 3 pt 3 linecolor rgb "red"
plot './xlabels.dat' with labels center offset 0, 1.5 point, 'output.hgrm' using 4:1 with lines
*/
            gnuPlot.WriteFile(".\\output.hgrm", stats.Histogram);
            gnuPlot.Set("terminal png", "output '.\\plot.png'", "logscale x");
            gnuPlot.Unset("xtics");
            gnuPlot.Set("key top left", "set style line 1 lt 1 lw 3 pt 3 linecolor rgb \"red\"");
            gnuPlot.Plot(".\\xlabels.dat", "with labels center offset 0, 1.5 point, 'output.hgrm' using 4:1 with lines");
        }

        private void OnStatusChange(ConnectionInfo connectionInfo)
        {
            Server = connectionInfo.Server;

            switch (connectionInfo.ConnectionStatus)
            {
                case ConnectionStatus.Uninitialized:
                case ConnectionStatus.Connecting:
                    Status = "Connecting...";
                    Disconnected = true;
                    break;
                case ConnectionStatus.Reconnected:
                case ConnectionStatus.Connected:
                    Status = "Connected";
                    Disconnected = false;
                    break;
                case ConnectionStatus.ConnectionSlow:
                    Status = "Slow connection detected";
                    Disconnected = false;
                    break;
                case ConnectionStatus.Reconnecting:
                    Status = "Reconnecting...";
                    Disconnected = true;
                    break;
                case ConnectionStatus.Closed:
                    Status = "Disconnected";
                    Disconnected = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string Status { get; private set; }
        public string Server { get; private set; }
        public bool Disconnected { get; private set; }
        public long UiUpdates { get; private set; }
        public long TicksReceived { get; private set; }
        public string TotalLatency { get; private set; }
        public string ServerClientLatency { get; private set; }
        public long UiLatency { get; private set; }
        public string Histogram { get; private set; }
        public string CpuTime { get; private set; }
        public string CpuPercent { get; private set; }
    }
}