using System;

namespace MarketAnalyzer.Crawler.Model
{
    public class JobRun
    {
        public long Id { get; set; }
        public DateTime RunDate { get; set; }
        public JobStatus Status { get; set; }
        public string DetailedMessage { get; set; }
    }
}
