using MarketAnalyzer.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace MarketAnalyzer.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<JobRun> JobRuns { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemIndicator> ItemIndicators { get; set; }
        public DbSet<ItemWeekIndicator> ItemWeekIndicators { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }
    }
}
