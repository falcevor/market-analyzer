using MarketAnalyzer.Crawler.Model;
using Microsoft.EntityFrameworkCore;

namespace MarketAnalyzer.Crawler
{
    public class AppDbContext : DbContext
    {
        public DbSet<JobRun> JobRuns { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemIndicator> ItemIndicators { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }
    }
}
