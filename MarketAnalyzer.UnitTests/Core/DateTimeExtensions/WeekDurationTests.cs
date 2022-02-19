using FluentAssertions;
using MarketAnalyzer.Core.Extensions;
using System;
using Xunit;

namespace MarketAnalyzer.UnitTests.Core.DateTimeExtensions
{
    public class WeekDurationTests
    {
        [Fact]
        public void Full_week()
        {
            new DateTime(2022, 1, 28).WeekDuration().Should().Be(7);
            new DateTime(2022, 1, 15).WeekDuration().Should().Be(7);

            new DateTime(2022, 2, 1).WeekDuration().Should().Be(7);
            new DateTime(2022, 2, 3).WeekDuration().Should().Be(7);
            new DateTime(2022, 2, 8).WeekDuration().Should().Be(7);
            new DateTime(2022, 2, 12).WeekDuration().Should().Be(7);
            new DateTime(2022, 2, 20).WeekDuration().Should().Be(7);
            new DateTime(2022, 2, 25).WeekDuration().Should().Be(7);
            new DateTime(2022, 2, 28).WeekDuration().Should().Be(7);
        }

        [Fact]
        public void Partial_week()
        {
            new DateTime(2022, 1, 29).WeekDuration().Should().Be(3);
            new DateTime(2022, 1, 30).WeekDuration().Should().Be(3);
            new DateTime(2022, 1, 31).WeekDuration().Should().Be(3);

            new DateTime(2022, 3, 29).WeekDuration().Should().Be(3);
            new DateTime(2022, 3, 30).WeekDuration().Should().Be(3);
            new DateTime(2022, 3, 31).WeekDuration().Should().Be(3);

            new DateTime(2022, 4, 29).WeekDuration().Should().Be(2);
            new DateTime(2022, 4, 30).WeekDuration().Should().Be(2);
        }
    }
}
