using MarketAnalyzer.Core.Model;

namespace MarketAnalyzer.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsWeekInPastFrom(this DateTime lastDate, DateTime current)
        {
            if (lastDate >= DateTime.Now) return false;

            return (lastDate.Day - 1) / 7 != (current.Day - 1) / 7
                || lastDate.Month != current.Month
                || lastDate.Year != current.Year;
        }

        public static DateTime FirstDayOfWeek(this DateTime date)
        {
            var week = (date.Day - 1) / 7;
            var firstDay = 1 + week * 7;
            return new DateTime(date.Year, date.Month, firstDay);
        }

        public static DateTime LastDayOfWeek(this DateTime date)
        {
            var firstDay = date.FirstDayOfWeek();
            return firstDay
                .AddDays(firstDay.WeekDuration() - 1);
        }

        public static int WeekDuration(this DateTime date)
        {
            var remains = DateTime.DaysInMonth(date.Year, date.Month) 
                - date.FirstDayOfWeek().Day + 1;
            return remains > 7 ? 7 : remains;
        }

        public static DateTimeInterval WeekBounds(this DateTime date)
        {
            var firstDay = date.FirstDayOfWeek();
            var lastDay = date.LastDayOfWeek().DayEnd();

            return new DateTimeInterval(firstDay, lastDay);
        }

        public static DateTime DayEnd(this DateTime date) 
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static DateTime FirstDayOfNextWeek(this DateTime date)
        {
            return date.LastDayOfWeek().AddDays(1);
        }
    }
}
