using MarketAnalyzer.Crawler.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MarketAnalyzer.Crawler
{
    public class AppDbContext : DbContext
    {
        public DbSet<JobRun> JobRuns { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemIndicator> ItemIndicators { get; set; }

        private string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_connectionString, options => options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
        }
    }
}
