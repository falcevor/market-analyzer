using MarketAnalyzer.Crawler.Services;
using MarketAnalyzer.Data.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MarketAnalyzer.Crawler
{
    internal class Startup
    {
        internal static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddDataLayer();
            services.AddHostedService<CrawlerService>();
        }
    }
}