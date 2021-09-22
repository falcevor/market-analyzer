using MarketAnalyzer.Crawler.Model;
using Microsoft.EntityFrameworkCore;

namespace MarketAnalyzer.Crawler
{
    public class AppDbContext : DbContext
    {
        public DbSet<JobRun> JobRuns { get; }
        public DbSet<Item> Items { get; }
        public DbSet<ItemIndicator> ItemIndicators { get; }

        private string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_connectionString);
        }
    }
}
