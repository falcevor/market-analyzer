using MarketAnalyzer.Core.Temporality;
using System;
using Xunit;
using FluentAssertions;
using MarketAnalyzer.Core.Model;

namespace MarketAnalyzer.UnitTests.Core.Temporality
{
    public class WeekIntervalProducerTests
    {
        private readonly WeekIntervalProducer _producer = new WeekIntervalProducer();

        [Fact]
        public void Full_month_week_intervals()
        {
            var intervals = _producer.Produce(new DateTime(2022, 2, 1), new DateTime(2022, 2, 28));
            intervals.Should().BeEquivalentTo(new []
            {
                new DateTimeInterval(new DateTime(2022, 2, 1), new DateTime(2022, 2, 7, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 2, 8), new DateTime(2022, 2, 14, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 2, 15), new DateTime(2022, 2, 21, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 2, 22), new DateTime(2022, 2, 28, 23, 59, 59))
            });

            intervals = _producer.Produce(new DateTime(2022, 1, 1), new DateTime(2022, 1, 31));
            intervals.Should().BeEquivalentTo(new[]
            {
                new DateTimeInterval(new DateTime(2022, 1, 1),  new DateTime(2022, 1, 7, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 1, 8),  new DateTime(2022, 1, 14, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 1, 15), new DateTime(2022, 1, 21, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 1, 22), new DateTime(2022, 1, 28, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 1, 29), new DateTime(2022, 1, 31, 23, 59, 59))
            });
        }

        [Fact]
        public void Partial_month_week_intervals()
        {
            var intervals = _producer.Produce(new DateTime(2022, 2, 5), new DateTime(2022, 2, 25));
            intervals.Should().BeEquivalentTo(new[]
            {
                new DateTimeInterval(new DateTime(2022, 2, 5), new DateTime(2022, 2, 7, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 2, 8), new DateTime(2022, 2, 14, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 2, 15), new DateTime(2022, 2, 21, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 2, 22), new DateTime(2022, 2, 25, 23, 59, 59))
            });

            intervals = _producer.Produce(new DateTime(2022, 2, 18), new DateTime(2022, 2, 25));
            intervals.Should().BeEquivalentTo(new[]
            {
                new DateTimeInterval(new DateTime(2022, 2, 18), new DateTime(2022, 2, 21, 23, 59, 59)),
                new DateTimeInterval(new DateTime(2022, 2, 22), new DateTime(2022, 2, 25, 23, 59, 59))
            });
        }
    }
}
