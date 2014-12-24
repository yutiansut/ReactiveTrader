using System.Collections.Generic;
using System.Linq;

namespace Adaptive.ReactiveTrader.Shared.DTO.Analytics
{
    public class PositionUpdatesDto
    {
        public PositionUpdatesDto()
        {
            CurrentPositions = Enumerable.Empty<CurrencyPairPositionDto>();
            History = Enumerable.Empty<HistoricPositionDto>();
        }
        public IEnumerable<CurrencyPairPositionDto> CurrentPositions { get; set; }
        public IEnumerable<HistoricPositionDto> History { get; set; }
    }
}