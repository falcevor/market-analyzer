﻿namespace MarketAnalyzer.Core.Model
{
    public class ItemIndicator : Entity
    {
        public long ItemId { get; set; }
        public Item Item { get; set; }
        public long JobRunId { get; set; }
        public JobRun JobRun { get; set; }

        public long Count { get; set; }
        public long TotalTrades { get; set; }
        public long BasePrice { get; set; }
        public long DailyVolume { get; set; }
    }
}