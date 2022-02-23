using MarketAnalyzer.Core.Extensions;
using MarketAnalyzer.Crawler.Jobs;
using MarketAnalyzer.Data.Extensions;
using MarketAnalyzer.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;

namespace MarketAnalyzer.Crawler
{
    internal class Startup
    {
        internal static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddDomainLayer();
            services.AddApplicationLayer();
            services.AddDataLayer();

            var crawlerJobKey = new JobKey("MarketCrawlerJob");
            var aggregationJobKey = new JobKey("AggregationJob");

            services.AddQuartz(config => config
                .AddJob<CrawlerJob>(job => job
                    .WithDescription("Market data crawler job")
                    .WithIdentity(crawlerJobKey)
                )

                .AddTrigger(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(schedule => schedule
                        .WithInterval(TimeSpan.FromHours(4))
                        .RepeatForever()
                    )
                    .ForJob(crawlerJobKey)
                )


                .AddJob<AggregationJob>(job => job
                    .WithDescription("Week aggregator job")
                    .WithIdentity(aggregationJobKey)
                )

                .AddTrigger(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(schedule => schedule
                        .WithInterval(TimeSpan.FromHours(12))
                        .RepeatForever()
                    )
                    .ForJob(aggregationJobKey)
                )
                .UseMicrosoftDependencyInjectionJobFactory()
            );
            
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
        }
    }
}