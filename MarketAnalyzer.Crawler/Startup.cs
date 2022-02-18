using MarketAnalyzer.Crawler.Jobs;
using MarketAnalyzer.Data.Extensions;
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
            services.AddDataLayer();

            var jobKey = new JobKey("MarketCrawlerJob");

            services.AddQuartz(config => config
                .AddJob<CrawlerJob>(job => job
                    .WithDescription("Market data crawler job")
                    .WithIdentity(jobKey)
                )

                .AddTrigger(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(schedule => schedule
                        .WithInterval(TimeSpan.FromHours(4))
                        .RepeatForever()
                    )
                    .ForJob(jobKey)
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