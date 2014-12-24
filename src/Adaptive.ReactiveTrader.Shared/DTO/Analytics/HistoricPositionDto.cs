using System;
using Newtonsoft.Json.Serialization;

namespace Adaptive.ReactiveTrader.Shared.DTO.Analytics
{
    public class HistoricPositionDto
    {

        public DateTimeOffset Timestamp { get; set; }
        public decimal UsdPnl { get; set; }
    }
}