namespace MarketAnalyzer.Domain.Model
{
    public class ItemAggregatedStatistic
    {
        public long MaxCount { get; set; }
        public long MinCount { get; set; }
        public long TradesCount { get; set; }
        public long MaxPrice { get; set; }
        public long MinPrice { get; set; }
        public long MinDailyVolume { get; set; }
        public long MaxDailyVolume { get; set; }
        public long AvgDailyVolume { get; set; }
    }
}
