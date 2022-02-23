using System;

namespace MarketAnalyzer.Core.Model
{
    public class JobRun : Entity
    {
        public DateTime RunDate { get; set; }
        public JobStatus Status { get; set; }
        public string DetailedMessage { get; set; }
    }
}
