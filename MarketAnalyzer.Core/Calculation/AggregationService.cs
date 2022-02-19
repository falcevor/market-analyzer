using MarketAnalyzer.Core.Extensions;
using MarketAnalyzer.Data;
using MarketAnalyzer.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace MarketAnalyzer.Core.Calculation
{
    internal class AggregationService : IAggregationService
    {
        private readonly IStatisticAggregator _aggregationService;
        private readonly AppDbContext _dbContext; // TODO: INVERSE DEPENDENCY!

        public AggregationService(IStatisticAggregator aggregationService, 
            AppDbContext dbContext)
        {
            _aggregationService = aggregationService;
            _dbContext = dbContext;
        }

        public async Task AggregateItemIndicatorsByWeekAsync()
        {
            var lastDate = await GetLastAggregationDate();

            while (lastDate.IsWeekInPastFrom(DateTime.Now))
            {
                lastDate = lastDate.MoveToFirstDayOfNextWeek();
                (var dateFrom, var dateTo) = lastDate.WeekBounds();
                await AggregateInternal(dateFrom, dateTo);
            }
        }
        
        private async Task<DateTime> GetLastAggregationDate()
        {
            if (!_dbContext.ItemWeekIndicators.Any())
                return await _dbContext.JobRuns.Select(x => x.RunDate).MinAsync();

            return await _dbContext.ItemWeekIndicators.Select(x => x.StartDate).MaxAsync();
        }

        private Task AggregateInternal(DateTime dateFrom, DateTime dateTo)
        {
            // TODO: implement calculation invocation and saving to storage
            throw new NotImplementedException();
        }
    }
}
