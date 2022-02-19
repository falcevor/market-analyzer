using System;
using Xunit;
using FluentAssertions;
using MarketAnalyzer.Core.Extensions;

namespace MarketAnalyzer.UnitTests.Core.DateTimeExtensions
{
    public class IsWeekInPastFromTests
    {
        [Fact]
        public void Past_week_in_past_month()
        {
            var date = DateTime.Now.AddMonths(-1);
            date.IsWeekInPastFrom(DateTime.Now).Should().BeTrue();
        }

        [Fact]
        public void Past_week_in_past_year()
        {
            var date = DateTime.Now.AddYears(-1);
            date.IsWeekInPastFrom(DateTime.Now).Should().BeTrue();
        }

        [Fact]
        public void Future_week_is_not_in_the_past()
        {
            var date = DateTime.Now.AddDays(7);
            date.IsWeekInPastFrom(DateTime.Now).Should().BeFalse();
        }

        [Fact]
        public void Today_is_not_past_week()
        {
            var date = DateTime.Now;
            date.IsWeekInPastFrom(DateTime.Now).Should().BeFalse();
        }

        [Fact]
        public void Same_week()
        {
            var date = new DateTime(2022, 2, 1);
            date.IsWeekInPastFrom(new DateTime(2022, 2, 3)).Should().BeFalse();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 6)).Should().BeFalse();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 7)).Should().BeFalse();

            date = new DateTime(2022, 2, 8);
            date.IsWeekInPastFrom(new DateTime(2022, 2, 10)).Should().BeFalse();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 12)).Should().BeFalse();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 14)).Should().BeFalse();

            date = new DateTime(2022, 2, 15);
            date.IsWeekInPastFrom(new DateTime(2022, 2, 17)).Should().BeFalse();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 19)).Should().BeFalse();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 21)).Should().BeFalse();

            date = new DateTime(2022, 2, 22);
            date.IsWeekInPastFrom(new DateTime(2022, 2, 24)).Should().BeFalse();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 26)).Should().BeFalse();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 28)).Should().BeFalse();
        }

        [Fact]
        public void End_of_last_month()
        {
            var date = new DateTime(2022, 1, 29);
            date.IsWeekInPastFrom(new DateTime(2022, 2, 1)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 2)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 3)).Should().BeTrue();

            date = new DateTime(2022, 1, 31);
            date.IsWeekInPastFrom(new DateTime(2022, 2, 1)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 2)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 3)).Should().BeTrue();
        }

        [Fact]
        public void Same_month_past_week()
        {
            var date = new DateTime(2022, 2, 1);
            date.IsWeekInPastFrom(new DateTime(2022, 2, 8)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 10)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 14)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 25)).Should().BeTrue();

            date = new DateTime(2022, 2, 8);
            date.IsWeekInPastFrom(new DateTime(2022, 2, 15)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 19)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 26)).Should().BeTrue();

            date = new DateTime(2022, 2, 15);
            date.IsWeekInPastFrom(new DateTime(2022, 2, 22)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 24)).Should().BeTrue();
            date.IsWeekInPastFrom(new DateTime(2022, 2, 28)).Should().BeTrue();
        }
    }
}
