using Microsoft.Extensions.Configuration;
using StackExchange.Utils;
using Microsoft.Extensions.Hosting;
using MarketAnalyzer.Data.Extensions;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace MarketAnalyzer.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateDefaultHostBuilder().Build();
            host.MigrateDbContexts();
            host.Run();
        }

        private static IHostBuilder CreateDefaultHostBuilder() =>
            new HostBuilder()
                .ConfigureAppConfiguration(builder => builder
                    .WithPrefix("env", envBuilder => envBuilder
                        .AddEnvironmentVariables()
                    )
                    .WithSubstitution(subsBuilder => subsBuilder
                        .AddJsonFile("/app/Configuration/appsettings.json", true)
                        .AddEnvironmentVariables()
                    )
                )
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    var configBuilder = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Console();

                    builder.AddSerilog(configBuilder.CreateLogger());
                })
                .ConfigureServices(Startup.ConfigureServices);
    }
}
