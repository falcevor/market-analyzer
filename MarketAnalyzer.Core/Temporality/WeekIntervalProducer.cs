using MarketAnalyzer.Core.Model;
using MarketAnalyzer.Core.Extensions;

namespace MarketAnalyzer.Core.Temporality
{
    public class WeekIntervalProducer
    {
        public IEnumerable<DateTimeInterval> Produce(DateTime from, DateTime to)
        {
            var first = from;
            var last = from.LastDayOfWeek();

            while (first <= to)
            {
                last = last > to ? to : last;
                yield return new DateTimeInterval(first, last.DayEnd());

                first = first.FirstDayOfNextWeek();
                last = first.LastDayOfWeek();
            }
        }
    }
}
