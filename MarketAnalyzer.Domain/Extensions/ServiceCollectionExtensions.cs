using MarketAnalyzer.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MarketAnalyzer.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainLayer(this IServiceCollection services)
        {
            services.AddTransient<IStatisticAggregator, StatisticAggregator>();
            return services;
        }
    }
}
