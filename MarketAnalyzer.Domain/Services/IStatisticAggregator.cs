using MarketAnalyzer.Domain.Model;
using System.Collections.Generic;

namespace MarketAnalyzer.Domain.Services
{
    public interface IStatisticAggregator
    {
        ItemAggregatedStatistic Aggregate(IEnumerable<ItemStatistic> statistics);
    }
}
