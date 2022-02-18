using MarketAnalyzer.Domain.Model;
using MarketAnalyzer.Domain.Services;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace MarketAnalyzer.UnitTests.Domain
{
    public class StatisticAggregatorTests
    {
        private readonly IStatisticAggregator _aggregator = new StatisticAggregator();
        private readonly IEnumerable<ItemStatistic> _testData;

        public StatisticAggregatorTests()
        {
            var dataContent = File.ReadAllText(@".\Domain\StatisticAggregatorTests_Data.json");
            _testData = JsonSerializer.Deserialize<ItemStatistic[]>(dataContent);
        }

        [Fact]
        public void Return_empty_result_for_null_input()
        {
            var result = _aggregator.Aggregate(null);
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new ItemAggregatedStatistic());
        }

        [Fact]
        public void Return_empty_result_for_empty_input()
        {
            var nullResult = _aggregator.Aggregate(new ItemStatistic[0]);
            nullResult.Should().NotBeNull();
            nullResult.Should().BeEquivalentTo(new ItemAggregatedStatistic());
        }

        [Fact]
        public void Max_price_calculated_properly() 
        {
            var result = _aggregator.Aggregate(_testData);
            result.MaxPrice.Should().Be(65000);
        }

        [Fact]
        public void Min_price_calculated_properly()
        {
            var result = _aggregator.Aggregate(_testData);
            result.MinPrice.Should().Be(52000);
        }

        [Fact]
        public void Max_count_calculated_properly()
        {
            var result = _aggregator.Aggregate(_testData);
            result.MaxCount.Should().Be(14);
        }

        [Fact]
        public void Min_count_calculated_properly()
        {
            var result = _aggregator.Aggregate(_testData);
            result.MinCount.Should().Be(0);
        }

        [Fact]
        public void Trades_count_calculated_properly()
        {
            var result = _aggregator.Aggregate(_testData);
            result.TradesCount.Should().Be(90);
        }

        [Fact]
        public void Max_daily_volume_calculated_properly()
        {
            var result = _aggregator.Aggregate(_testData);
            result.MaxDailyVolume.Should().Be(43);
        }

        [Fact]
        public void Min_daily_volume_calculated_properly()
        {
            var result = _aggregator.Aggregate(_testData);
            result.MinDailyVolume.Should().Be(0);
        }

        [Fact]
        public void Avg_daily_volume_calculated_properly()
        {
            var result = _aggregator.Aggregate(_testData);
            result.AvgDailyVolume.Should().Be(14);
        }
    }
}
