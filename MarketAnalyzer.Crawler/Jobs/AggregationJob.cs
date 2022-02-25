using MarketAnalyzer.Core.Calculation;
using MarketAnalyzer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;

namespace MarketAnalyzer.Crawler.Jobs
{
    public class AggregationJob : IJob
    {
        public IServiceProvider _serviceProvider;

        public AggregationJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IAggregationService>();
            await service.AggregateItemIndicatorsByWeekAsync(DateTime.Now);
        }
    }
}
