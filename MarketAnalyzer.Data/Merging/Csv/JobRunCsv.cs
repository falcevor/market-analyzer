using System;

namespace MarketAnalyzer.Data.Merging.Csv
{
    public class JobRunCsv
    {
        public long Id { get; set; }
        public DateTime RunDate { get; set; }
        public int Status { get; set; }
        public string DetailedMessage { get; set; }
    }
}
