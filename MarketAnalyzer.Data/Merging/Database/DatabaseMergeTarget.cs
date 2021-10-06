using MarketAnalyzer.Data.Model;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Threading.Tasks;

namespace MarketAnalyzer.Data.Merging.Database
{
    public class DatabaseMergeTarget : IMergeTarget
    {
        private readonly AppDbContext _context;

        public DatabaseMergeTarget(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddItemAsync(Item item)
        {
            await _context.AddAsync(item);
        }

        public async Task AddItemIndicatorAsync(ItemIndicator itemIndicator)
        {
            await _context.AddAsync(itemIndicator);
        }

        public async Task AddJobRunAsync(JobRun jobRun)
        {
            await _context.AddAsync(jobRun);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task TruncateAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(attempt * 5)
                )
                .ExecuteAsync(async () => await _context.Database.MigrateAsync());
        }
    }
}
