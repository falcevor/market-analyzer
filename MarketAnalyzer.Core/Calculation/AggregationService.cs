using AutoMapper;
using MarketAnalyzer.Core.Abstraction;
using MarketAnalyzer.Core.Extensions;
using MarketAnalyzer.Core.Model;
using MarketAnalyzer.Core.Temporality;
using MarketAnalyzer.Domain.Model;
using MarketAnalyzer.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace MarketAnalyzer.Core.Calculation
{
    public class AggregationService : IAggregationService
    {
        private readonly IStatisticAggregator _service;
        private readonly IMapper _mapper;
        private readonly WeekIntervalProducer _weekProducer;

        private readonly IStore<ItemIndicator> _indicatorStore;
        private readonly IStore<ItemWeekIndicator> _weekIndicatorStore;
        private readonly IStore<JobRun> _jobRunStore;

        public AggregationService(IStatisticAggregator service,
            IMapper mapper,
            IStore<ItemIndicator> indicatorStore, 
            IStore<ItemWeekIndicator> weekIndicatorStore, 
            IStore<JobRun> jobRunStore)
        {
            _service = service;
            _mapper = mapper;
            _indicatorStore = indicatorStore;
            _weekIndicatorStore = weekIndicatorStore;
            _jobRunStore = jobRunStore;
            _weekProducer = new WeekIntervalProducer();
        }

        public async Task AggregateItemIndicatorsByWeekAsync()
        {
            var lastDate = await GetLastAggregationDate();

            if (!lastDate.IsWeekInPastFrom(DateTime.Now))
                return;

            var weekIntervals = _weekProducer.Produce(lastDate, DateTime.Now.FirstDayOfWeek().AddDays(-1));

            foreach (var interval in weekIntervals)
            {
                await AggregateWeekInternal(interval.From, interval.To);
            }
        }
        
        private Task<DateTime> GetLastAggregationDate()
        {
            if (!_weekIndicatorStore.Values.Any())
                return Task.FromResult(_jobRunStore.Values.Select(x => x.RunDate).Min());
            
            return Task.FromResult(_weekIndicatorStore.Values.Select(x => x.StartDate).Max());
        }

        private async Task AggregateWeekInternal(DateTime dateFrom, DateTime dateTo)
        {
            var weekIndicators = _indicatorStore.Values
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
                await _weekIndicatorStore.Add(result);
            }
        }

        private ItemWeekIndicator AggregateItem(IEnumerable<ItemStatistic> items)
        {
            var aggregated = _service.Aggregate(items);
            var result = _mapper.Map<ItemWeekIndicator>(aggregated);
            return result;
        }
    }
}
