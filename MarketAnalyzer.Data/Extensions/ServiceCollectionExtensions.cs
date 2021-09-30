using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MarketAnalyzer.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var config = provider.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("Default");

            services.AddDbContextFactory<AppDbContext>(builder =>
                builder.UseNpgsql(connectionString, options =>
                    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
                )
            );
            return services;
        }
    }
}
