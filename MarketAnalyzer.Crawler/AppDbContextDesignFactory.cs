using Microsoft.EntityFrameworkCore.Design;

namespace MarketAnalyzer.Crawler
{
    public class AppDbContextDesignFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var context = new AppDbContext("stub");
            return context;
        }
    }
}
