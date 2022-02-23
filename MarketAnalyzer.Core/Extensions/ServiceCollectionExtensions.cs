using MarketAnalyzer.Core.Calculation;
using MarketAnalyzer.Core.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace MarketAnalyzer.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(config => config.AddProfile<AggregateProfile>());
            services.AddSingleton<IAggregationService, AggregationService>();
            return services;
        }
    }
}
