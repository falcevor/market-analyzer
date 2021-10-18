using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace MarketAnalyzer.Data.Extensions
{
    public static class HostExtenisions
    {
        public static IHost MigrateDbContexts(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = host.Services.GetRequiredService<ILogger<DbContext>>();
            logger.LogInformation("Database migration process started.");

            var contextTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.BaseType == typeof(DbContext))
                .ToList();

            logger.LogInformation("DbContext from assembly to migrate {0}", 
                string.Join(", ", contextTypes.Select(x => x.FullName)));

            contextTypes.ForEach(contextType =>
            {
                var loggerType = typeof(ILogger<>).MakeGenericType(contextType);
                var logger = services.GetRequiredService(loggerType) as ILogger<DbContext>;

                var context = services.GetService(contextType) as DbContext;
                if (context is not null)
                {
                    CreateRetryPolicy(logger).Execute(() => context.Database.Migrate());
                    return;
                }

                var factoryType = typeof(IDbContextFactory<>).MakeGenericType(contextType);
                var factory = services.GetService(factoryType) as IDbContextFactory<DbContext>;
                if (factory is not null)
                    CreateRetryPolicy(logger).Execute(() => factory.CreateDbContext().Database.Migrate());
            });

            return host;
        }

        private static Policy CreateRetryPolicy<T>(ILogger<T> logger)
        {
            return Policy
                .Handle<DbException>()
                .WaitAndRetry(
                    retryCount: 15,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (ex, duration, attempt, context) =>
                        logger.LogWarning(ex, "Exception during context migration (attempt {attempt})",
                            attempt)
                );
        }
    }
}
