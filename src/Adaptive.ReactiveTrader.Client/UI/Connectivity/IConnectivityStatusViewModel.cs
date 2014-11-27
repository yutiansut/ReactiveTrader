using System;
using Adaptive.ReactiveTrader.Client.Domain.Instrumentation;
using Adaptive.ReactiveTrader.Client.UI.SpotTiles;

namespace Adaptive.ReactiveTrader.Client.UI.Connectivity
{
    public interface IConnectivityStatusViewModel
    {
        void OnStatistics(Statistics statistics, TimeSpan frequency);
        string Status { get; }
        string Server { get; }
        bool Disconnected { get; }
        
        long UiUpdates { get; }
        long TicksReceived { get; }
        string ServerClientLatency { get; }
        string TotalLatency { get; }
        long UiLatency { get; }
        string Histogram { get; }
        
        string CpuTime { get; }
        string CpuPercent { get; }
    }
}