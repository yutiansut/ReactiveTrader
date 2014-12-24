namespace Adaptive.ReactiveTrader.Shared.DTO.Analytics
{
    public class CurrencyPairPositionDto
    {
        public string Symbol { get; set; }
        public decimal BasePnl { get; set; }
        public decimal BaseTradedAmount { get; set; }
    }
}