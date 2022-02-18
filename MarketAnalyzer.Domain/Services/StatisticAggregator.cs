using MarketAnalyzer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketAnalyzer.Domain.Services
{
    public class StatisticAggregator : IStatisticAggregator
    {
        public ItemAggregatedStatistic Aggregate(IEnumerable<ItemStatistic> statistics)
        {
            var result = new ItemAggregatedStatistic();
            
            if (statistics is null || !statistics.Any()) 
                return result;

            result.MaxPrice = CalculateMaxPrice(statistics);
            result.MinPrice = CalculateMinPrice(statistics);
            result.MaxCount = CalculateMaxCount(statistics);
            result.MinCount = CalculateMinCount(statistics);
            result.TradesCount = CalculateTradesCount(statistics);
            result.MaxDailyVolume = CalculateMaxDailyVolume(statistics);
            result.MinDailyVolume = CalculateMinDailyVolume(statistics);
            result.AvgDailyVolume = CalculateAvgDailyVolume(statistics);

            return result;
        }

        private long CalculateAvgDailyVolume(IEnumerable<ItemStatistic> statistics)
        {
            return statistics.Select(x => x.DailyVolume).Sum() / statistics.Count();
        }

        private long CalculateMaxDailyVolume(IEnumerable<ItemStatistic> statistics)
        {
            return statistics.Select(x => x.DailyVolume).Max();
        }

        private long CalculateMinDailyVolume(IEnumerable<ItemStatistic> statistics)
        {
            return statistics.Select(x => x.DailyVolume).Min();
        }

        private long CalculateTradesCount(IEnumerable<ItemStatistic> statistics)
        {
            return statistics.Select(x => x.TotalTrades).Max() 
                - statistics.Select(x => x.TotalTrades).Min();
        }

        private long CalculateMinCount(IEnumerable<ItemStatistic> statistics)
        {
            return statistics.Select(x => x.Count).Min();
        }

        private long CalculateMaxCount(IEnumerable<ItemStatistic> statistics)
        {
            return statistics.Select(x => x.Count).Max();
        }

        private long CalculateMinPrice(IEnumerable<ItemStatistic> statistics)
        {
            return statistics.Select(x => x.BasePrice).Min();
        }

        private long CalculateMaxPrice(IEnumerable<ItemStatistic> statistics)
        {
            return statistics.Select(x => x.BasePrice).Max();
        }
    }
}
