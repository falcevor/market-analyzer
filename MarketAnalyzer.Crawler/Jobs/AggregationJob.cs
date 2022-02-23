using MarketAnalyzer.Core.Calculation;
using Quartz;
using System.Threading.Tasks;

namespace MarketAnalyzer.Crawler.Jobs
{
    public class AggregationJob : IJob
    {
        public IAggregationService _service;

        public AggregationJob(IAggregationService service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _service.AggregateItemIndicatorsByWeekAsync();
        }
    }
}
