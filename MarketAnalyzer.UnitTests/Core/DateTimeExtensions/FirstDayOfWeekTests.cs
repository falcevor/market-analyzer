using FluentAssertions;
using MarketAnalyzer.Core.Extensions;
using System;
using Xunit;

namespace MarketAnalyzer.UnitTests.Core.DateTimeExtensions
{
    public class FirstDayOfWeekTests
    {
        [Fact]
        public void This_day_is_first_day()
        {
            var date = new DateTime(2022, 2, 1);
            date.FirstDayOfWeek().Should().Be(date);

            date = new DateTime(2022, 2, 8);
            date.FirstDayOfWeek().Should().Be(date);

            date = new DateTime(2022, 2, 15);
            date.FirstDayOfWeek().Should().Be(date);

            date = new DateTime(2022, 2, 22);
            date.FirstDayOfWeek().Should().Be(date);
        }

        [Fact]
        public void This_day_is_second_day()
        {
            var date = new DateTime(2022, 2, 2);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 2, 1));

            date = new DateTime(2022, 2, 9);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 2, 8));

            date = new DateTime(2022, 2, 16);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 2, 15));

            date = new DateTime(2022, 2, 23);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 2, 22));
        }

        [Fact]
        public void This_day_is_last_day()
        {
            var date = new DateTime(2022, 2, 7);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 2, 1));

            date = new DateTime(2022, 2, 14);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 2, 8));

            date = new DateTime(2022, 2, 21);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 2, 15));

            date = new DateTime(2022, 2, 28);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 2, 22));
        }

        [Fact]
        public void Partial_week()
        {
            var date = new DateTime(2022, 1, 31);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 1, 29));
            date = new DateTime(2022, 1, 30);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 1, 29));
            date = new DateTime(2022, 1, 29);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 1, 29));

            date = new DateTime(2022, 4, 30);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 4, 29));
            date = new DateTime(2022, 4, 29);
            date.FirstDayOfWeek().Should().Be(new DateTime(2022, 4, 29));
        }
    }
}
