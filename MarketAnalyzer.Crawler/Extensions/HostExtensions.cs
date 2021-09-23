using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace MarketAnalyzer.Crawler.Extensions
{
    public static class HostExtenisions
    {
        public static IHost MigrateDbContexts(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<DbContext>>();

            var contextTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.BaseType == typeof(DbContext))
                .ToList();

            contextTypes.ForEach(contextType =>
            {
                var context = services.GetService(contextType) as DbContext;
                if (context is not null)
                    CreateRetryPolicy(logger).Execute(() => context.Database.Migrate());
            });

            return host;
        }

        private static Policy CreateRetryPolicy(ILogger<DbContext> logger)
        {
            return Policy
                .Handle<DbException>()
                .WaitAndRetry(
                    retryCount: 5,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(attempt),
                    onRetry: (ex, duration, attempt, context) =>
                        logger.LogWarning(ex, "Exception during context migration (attempt {attempt})",
                            attempt)
                );
        }
    }
}
