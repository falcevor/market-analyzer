using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using MarketAnalyzer.Data;
using MarketAnalyzer.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Quartz;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MarketAnalyzer.Crawler.Jobs
{
    public class CrawlerJob : IJob
    {
        private static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };

        private const string spreadsheetId = "1ox0o-r9Ybff6UZk_4N4-Lh23m-NQvZ3aKvGCBWWbS8o";
        private const string range = "Sheet1!A1:F";

        private IConfiguration _config;
        private ILogger<CrawlerJob> _logger;
        private IDbContextFactory<AppDbContext> _dbContextFactory;

        public CrawlerJob(
            IConfiguration config,
            ILogger<CrawlerJob> logger,
            IDbContextFactory<AppDbContext> dbContextFactory
        )
        {
            _config = config;
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Policy.Handle<Exception>()
                .WaitAndRetryForeverAsync(
                    sleepDurationProvider: attempt => TimeSpan.FromMinutes(attempt),
                    onRetry: (excetion, attempt) => _logger.LogWarning("Job retrying after failure. Attempt: {attempt}", attempt)
                )
                .ExecuteAsync(async () => await ExecuteAttempt());
        }

        public async Task ExecuteAttempt()
        {
            var dbContext = _dbContextFactory.CreateDbContext();

            var jobRun = new JobRun()
            {
                RunDate = DateTime.Now,
                Status = JobStatus.Processing
            };
            dbContext.Add(jobRun);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Job started! Run id: {jobRunId}", jobRun.Id);

            try
            {
                await CrawlData(dbContext, jobRun);
            }
            catch (Exception ex)
            {
                jobRun.DetailedMessage = ex.Message;
                jobRun.Status = JobStatus.Failure;
                dbContext.Update(jobRun);
                dbContext.SaveChanges();
                _logger.LogError(ex, "Job {jobRunId} got some error :(", jobRun.Id);

            }

            _logger.LogInformation("Job finished! Run id: {jobRunId}", jobRun.Id);
        }

        private async Task CrawlData(AppDbContext context, JobRun jobRun)
        {
            var service = GetSheetsService();

            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            var response = await request.ExecuteAsync();
            var values = response.Values;

            if (values != null && values.Count > 0)
            {
                //New: Daily volume has been added in column F. It shows the average trades per day past 30 days.
                foreach (var row in values)
                {
                    var name = row[0].ToString();
                    var id = long.Parse(row[1].ToString());
                    var count = long.Parse(row[2].ToString());
                    var totalTrades = long.Parse(row[3].ToString());
                    var basePrice = long.Parse(row[4].ToString());
                    var dailyVolume = long.Parse(row[5].ToString());

                    await CreateItem(context, id, name);
                    await CreateItemIndicator(context, jobRun.Id, id, count, totalTrades, basePrice, dailyVolume);
                }
            }
            else
            {
                _logger.LogWarning("No information in Google Sheet");
            }

            jobRun.Status = JobStatus.Success;
            context.Update(jobRun);
            await context.SaveChangesAsync();
        }

        private static SheetsService GetSheetsService()
        {
            using (var stream = new FileStream("/app/Configuration/credentials.json", FileMode.Open, FileAccess.Read))
            {
                var serviceInitializer = new BaseClientService.Initializer
                {
                    HttpClientInitializer = GoogleCredential.FromStream(stream).CreateScoped(Scopes)
                };
                return new SheetsService(serviceInitializer);
            }
        }

        private async Task CreateItemIndicator(
          AppDbContext context,
          long jobRunId,
          long id,
          long count,
          long totalTrades,
          long basePrice,
          long dailyVolume)
        {
            await context.AddAsync(new ItemIndicator()
            {
                ItemId = id,
                JobRunId = jobRunId,
                BasePrice = basePrice,
                Count = count,
                TotalTrades = totalTrades,
                DailyVolume = dailyVolume
            });
        }

        private async Task CreateItem(AppDbContext context, long id, string name)
        {
            var item = await context.Items.FindAsync(id);
            if (item == null)
                await context.AddAsync(new Item()
                {
                    Id = id,
                    Name = name,
                    RegistrationDate = DateTime.Now
                });
        }
    }
}
