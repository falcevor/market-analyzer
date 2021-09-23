using MarketAnalyzer.Crawler.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace MarketAnalyzer.Crawler
{
    internal class Startup
    {
        internal static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var config = context.Configuration;
            var connectionString = config.GetConnectionString("Default");

            services.AddDbContextFactory<AppDbContext>(builder => 
                builder.UseNpgsql(connectionString, options => 
                    options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
                )
            );

            services.AddHostedService<CrawlerService>();
        }
    }
}