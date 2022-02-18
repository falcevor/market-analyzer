namespace MarketAnalyzer.Domain.Model
{
    public class ItemStatistic
    {
        public long Count { get; set; }
        public long TotalTrades { get; set; }
        public long BasePrice { get; set; }
        public long DailyVolume { get; set; }
    }
}
