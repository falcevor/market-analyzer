namespace MarketAnalyzer.Data.Merging.Csv
{
    public class ItemIndicatorCsv
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long JobRunId { get; set; }
        
        public long Count { get; set; }
        public long TotalTrades { get; set; }
        public long BasePrice { get; set; }
        public long DailyVolume { get; set; }
    }
}
