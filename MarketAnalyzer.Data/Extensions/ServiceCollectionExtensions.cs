using MarketAnalyzer.Core.Abstraction;
using MarketAnalyzer.Data.Petsistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketAnalyzer.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var config = provider.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("Default");

            services.AddDbContextFactory<AppDbContext>(builder => builder
                .UseLazyLoadingProxies()
                .UseNpgsql(connectionString, options => options
                    .MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                )
            );

            services.AddScoped(typeof(IStore<>), typeof(Store<>));
            return services;
        }
    }
}
