using System;

namespace MarketAnalyzer.Data.Model
{
    public class ItemWeekIndicator
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public Item Item { get; set; }

        public DateTime StartDate { get; set; }
        public int Duration { get; set; }

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
