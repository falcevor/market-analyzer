using FluentAssertions;
using MarketAnalyzer.Core.Extensions;
using System;
using Xunit;

namespace MarketAnalyzer.UnitTests.Core.DateTimeExtensions
{
    public class LastDayOfWeekTests
    {
        [Fact]
        public void This_day_is_last_day()
        {
            var date = new DateTime(2022, 2, 7);
            date.LastDayOfWeek().Should().Be(date);

            date = new DateTime(2022, 2, 14);
            date.LastDayOfWeek().Should().Be(date);

            date = new DateTime(2022, 2, 21);
            date.LastDayOfWeek().Should().Be(date);

            date = new DateTime(2022, 2, 28);
            date.LastDayOfWeek().Should().Be(date);
        }

        [Fact]
        public void This_day_is_second_day()
        {
            var date = new DateTime(2022, 2, 2);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 2, 7));

            date = new DateTime(2022, 2, 9);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 2, 14));

            date = new DateTime(2022, 2, 16);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 2, 21));

            date = new DateTime(2022, 2, 23);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 2, 28));
        }

        [Fact]
        public void This_day_is_first_day()
        {
            var date = new DateTime(2022, 2, 1);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 2, 7));

            date = new DateTime(2022, 2, 8);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 2, 14));

            date = new DateTime(2022, 2, 15);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 2, 21));

            date = new DateTime(2022, 2, 22);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 2, 28));
        }

        [Fact]
        public void Partial_week()
        {
            var date = new DateTime(2022, 1, 31);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 1, 31));
            date = new DateTime(2022, 1, 30);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 1, 31));
            date = new DateTime(2022, 1, 29);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 1, 31));

            date = new DateTime(2022, 4, 30);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 4, 30));
            date = new DateTime(2022, 4, 29);
            date.LastDayOfWeek().Should().Be(new DateTime(2022, 4, 30));
        }
    }
}
