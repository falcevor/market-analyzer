namespace MarketAnalyzer.Core.Calculation
{
    public interface IAggregationService
    {
        Task AggregateItemIndicatorsByWeekAsync(DateTime runDate);
    }
}
