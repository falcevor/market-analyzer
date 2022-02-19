using FluentAssertions;
using MarketAnalyzer.Core.Extensions;
using System;
using Xunit;

namespace MarketAnalyzer.UnitTests.Core.DateTimeExtensions
{
    public class MoveToFirstDayOfNextWeekTests
    {
        [Fact]
        public void Same_month()
        {
            var date = new DateTime(2022, 2, 1);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 2, 8));
            date = new DateTime(2022, 2, 4);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 2, 8));
            date = new DateTime(2022, 2, 7);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 2, 8));

            date = new DateTime(2022, 2, 8);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 2, 15));
            date = new DateTime(2022, 2, 12);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 2, 15));
            date = new DateTime(2022, 2, 14);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 2, 15));

            date = new DateTime(2022, 1, 22);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 1, 29));
            date = new DateTime(2022, 1, 25);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 1, 29));
            date = new DateTime(2022, 1, 28);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 1, 29));
        }

        [Fact]
        public void Next_week_in_next_month()
        {
            var date = new DateTime(2022, 2, 22);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 3, 1));
            date = new DateTime(2022, 2, 25);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 3, 1));
            date = new DateTime(2022, 2, 28);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 3, 1));

            date = new DateTime(2022, 1, 29);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 2, 1));
            date = new DateTime(2022, 1, 30);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 2, 1));
            date = new DateTime(2022, 1, 31);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 2, 1));

            date = new DateTime(2022, 4, 29);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 5, 1));
            date = new DateTime(2022, 4, 30);
            date.MoveToFirstDayOfNextWeek().Should().Be(new DateTime(2022, 5, 1));
        }
    }
}
