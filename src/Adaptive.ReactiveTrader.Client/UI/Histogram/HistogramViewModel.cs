using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Concurrency;
using Adaptive.ReactiveTrader.Client.Concurrency;
using Adaptive.ReactiveTrader.Client.Domain.Instrumentation;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;
using Adaptive.ReactiveTrader.Shared.UI;

namespace Adaptive.ReactiveTrader.Client.UI.Histogram
{
    public class HistogramViewModel : ViewModelBase, IHistogramViewModel
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly EventLoopScheduler _eventLoopScheduler = new EventLoopScheduler();
        private readonly Func<ITextFileWriter> _gnuPlotFactory;
        private int _index = 0;
        
        public HistogramViewModel(Func<ITextFileWriter> gnuPlotFactory)
        {
            _gnuPlotFactory = gnuPlotFactory;
        }

        public void OnStatistics(Statistics statistics, SpotTileSubscriptionMode subscriptionMode)
        {
            _eventLoopScheduler.Schedule(() => Render(statistics, subscriptionMode));
        }

        private void Render(Statistics statistics, SpotTileSubscriptionMode subscriptionMode)
        {
            var file = string.Format(@".\plot{0}.png", _index);

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
            #1#*/
            
            gnuPlot.WriteFile(string.Format(@".\output-{0}-{1}-{2}.hgrm", _index, subscriptionMode, _stopwatch.Elapsed.ToString().Replace(':', '.')),
                statistics.Histogram);
            _stopwatch.Restart();
            //gnuPlot.WriteFile(".\\output.hgrm", statistics.Histogram);
            //gnuPlot.Set("terminal png", string.Format("output '{0}'", file), "logscale x");
            //gnuPlot.Unset("xtics");
            //gnuPlot.Set("key top left", "set style line 1 lt 1 lw 3 pt 3 linecolor rgb \"red\"");
            //gnuPlot.Plot(".\\xlabels.dat", "with labels center offset 0, 1.5 point, 'output.hgrm' using 4:1 with lines");
            //gnuPlot.Close();
            //gnuPlot.Dispose();
            _index++;
        }
       
        public string ImageSource { get; private set; }
    }
}