using FluentAssertions;
using MarketAnalyzer.Core.Extensions;
using System;
using Xunit;

namespace MarketAnalyzer.UnitTests.Core.DateTimeExtensions
{
    public class WeekBoundsTests
    {
        [Fact]
        public void Move_to_day_end()
        {
            var date = new DateTime(2022, 2, 19);
            date.MoveToDayEnd().Should().Be(new DateTime(2022, 2, 19, 23, 59, 59));
        }

        [Fact]
        public void Full_week_bounds()
        {
            var date = new DateTime(2022, 2, 1);
            date.WeekBounds().Should().Be(
                (new DateTime(2022, 2, 1),
                 new DateTime(2022, 2, 7).MoveToDayEnd())
            );

            date = new DateTime(2022, 2, 4);
            date.WeekBounds().Should().Be(
                (new DateTime(2022, 2, 1),
                 new DateTime(2022, 2, 7).MoveToDayEnd())
            );

            date = new DateTime(2022, 2, 7);
            date.WeekBounds().Should().Be(
                (new DateTime(2022, 2, 1),
                 new DateTime(2022, 2, 7).MoveToDayEnd())
            );


            date = new DateTime(2022, 2, 22);
            date.WeekBounds().Should().Be(
                (new DateTime(2022, 2, 22),
                 new DateTime(2022, 2, 28).MoveToDayEnd())
            );

            date = new DateTime(2022, 2, 28);
            date.WeekBounds().Should().Be(
                (new DateTime(2022, 2, 22),
                 new DateTime(2022, 2, 28).MoveToDayEnd())
            );
        }


        [Fact]
        public void Partial_week_bounds()
        {
            var date = new DateTime(2022, 1, 29);
            date.WeekBounds().Should().Be(
                (new DateTime(2022, 1, 29),
                 new DateTime(2022, 1, 31).MoveToDayEnd())
            );

            date = new DateTime(2022, 1, 30);
            date.WeekBounds().Should().Be(
                (new DateTime(2022, 1, 29),
                 new DateTime(2022, 1, 31).MoveToDayEnd())
            );

            date = new DateTime(2022, 1, 31);
            date.WeekBounds().Should().Be(
                (new DateTime(2022, 1, 29),
                 new DateTime(2022, 1, 31).MoveToDayEnd())
            );

            date = new DateTime(2022, 4, 29);
            date.WeekBounds().Should().Be(
                (new DateTime(2022, 4, 29),
                 new DateTime(2022, 4, 30).MoveToDayEnd())
            );

            date = new DateTime(2022, 4, 30);
            date.WeekBounds().Should().Be(
                (new DateTime(2022, 4, 29),
                 new DateTime(2022, 4, 30).MoveToDayEnd())
            );

        }
    }
}
