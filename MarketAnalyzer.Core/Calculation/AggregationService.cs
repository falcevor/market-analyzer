using AutoMapper;
using MarketAnalyzer.Core.Extensions;
using MarketAnalyzer.Core.Temporality;
using MarketAnalyzer.Data;
using MarketAnalyzer.Data.Model;
using MarketAnalyzer.Domain.Model;
using MarketAnalyzer.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace MarketAnalyzer.Core.Calculation
{
    internal class AggregationService : IAggregationService
    {
        private readonly IStatisticAggregator _service;
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory; // TODO: INVERSE DEPENDENCY!
        private readonly IMapper _mapper;
        private readonly WeekIntervalProducer _weekProducer;


        public AggregationService(IStatisticAggregator aggregationService,
            IMapper mapper, 
            IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _service = aggregationService;
            _mapper = mapper;
            _weekProducer = new WeekIntervalProducer();
            _dbContextFactory = dbContextFactory;
        }

        public async Task AggregateItemIndicatorsByWeekAsync()
        {
            var dbContext = _dbContextFactory.CreateDbContext();
            var lastDate = await GetLastAggregationDate(dbContext);

            if (!lastDate.IsWeekInPastFrom(DateTime.Now))
                return;

            var weekIntervals = _weekProducer.Produce(lastDate, DateTime.Now.FirstDayOfWeek().AddDays(-1));

            foreach (var interval in weekIntervals)
            {
                await AggregateWeekInternal(dbContext, interval.From, interval.To);
            }
        }
        
        private async Task<DateTime> GetLastAggregationDate(AppDbContext dbContext)
        {
            if (!dbContext.ItemWeekIndicators.Any())
                return await dbContext.JobRuns.Select(x => x.RunDate).MinAsync();

            return await dbContext.ItemWeekIndicators.Select(x => x.StartDate).MaxAsync();
        }

        private async Task AggregateWeekInternal(AppDbContext dbContext, DateTime dateFrom, DateTime dateTo)
        {
            var weekIndicators = dbContext.ItemIndicators
                .Include(x => x.JobRun)
                .AsNoTracking()
                .Where(x => x.JobRun.RunDate >= dateFrom && x.JobRun.RunDate <= dateTo)
                .ToList();

            var items = weekIndicators.GroupBy(x => x.ItemId);

            foreach (var item in items)
            {
                var result = AggregateItem(item.Select(x => _mapper.Map<ItemStatistic>(x)));
                result.ItemId = item.Key;
                result.StartDate = dateFrom;
                result.Duration = (dateTo - dateFrom).Days;
                await dbContext.ItemWeekIndicators.AddAsync(result);
            }
            await dbContext.SaveChangesAsync();
        }

        private ItemWeekIndicator AggregateItem(IEnumerable<ItemStatistic> items)
        {
            var aggregated = _service.Aggregate(items);
            var result = _mapper.Map<ItemWeekIndicator>(aggregated);
            return result;
        }
    }
}
