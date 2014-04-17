using System.Collections.Generic;
using System.Linq;
using Adaptive.ReactiveTrader.Shared.DTO.Execution;

namespace Adaptive.ReactiveTrader.Server.Blotter
{
    public class TradeRepository : ITradeRepository
    {
        private readonly Queue<TradeDto> _trades = new Queue<TradeDto>();
        private const int MaxTrades = 50;

        public void StoreTrade(TradeDto trade)
        {
            lock (_trades)
            {
                _trades.Enqueue(trade);

                if (_trades.Count > MaxTrades)
                {
                    _trades.Dequeue();
                }
            }
        }

        public IList<TradeDto> GetAllTrades()
        {
            IList<TradeDto> trades;

            lock (_trades)
            {
                trades = _trades.ToList();
            }

            return trades;
        } 
    }
}