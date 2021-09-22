using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using MarketAnalyzer.Crawler.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MarketAnalyzer.Crawler
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        static void Main(string[] args)
        {
            var context = new AppDbContext("User ID=postgres;Password=123456;Host=postgres-marketanalyzer;Port=5432;Database=market-analyzer;Pooling=true;Connection Lifetime=0;");
            context.Database.Migrate();

            var jobRun = new JobRun()
            {
                RunDate = DateTime.Now,
                Status = JobStatus.Processing
            };
            context.Add(jobRun);
            context.SaveChanges();
            Console.WriteLine($"Job run id: {jobRun.Id}");

            try
            {
                CrawlData(context, jobRun);
            } 
            catch(Exception ex)
            {
                jobRun.DetailedMessage = ex.Message;
                jobRun.Status = JobStatus.Failure;
                context.Update(jobRun);
                context.SaveChanges();
            }

            Console.Read();
        }

        private static void CrawlData(AppDbContext context, JobRun jobRun)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                var secrets = GoogleClientSecrets.FromStream(stream);
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    secrets.Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1ox0o-r9Ybff6UZk_4N4-Lh23m-NQvZ3aKvGCBWWbS8o";
            String range = "Sheet1!A1:F";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                //New: Daily volume has been added in column F. It shows the average trades per day past 30 days.
                Console.WriteLine("Name, Index, Count, TotalTrades, BasePrice, DailyVolume");
                foreach (var row in values)
                {
                    var name = row[0].ToString();
                    var id = long.Parse(row[1].ToString());
                    var count = long.Parse(row[2].ToString());
                    var totalTrades = long.Parse(row[3].ToString());
                    var basePrice = long.Parse(row[4].ToString());
                    var dailyVolume = long.Parse(row[5].ToString());

                    CreateItem(context, id, name);
                    CreateItemIndicator(context, jobRun.Id, id, count, totalTrades, basePrice, dailyVolume);
                    
                    Console.WriteLine(id);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            jobRun.Status = JobStatus.Success;
            context.Update(jobRun);
            context.SaveChanges();
        }

        private static void CreateItemIndicator(
            AppDbContext context, 
            long jobRunId, 
            long id, 
            long count, 
            long totalTrades, 
            long basePrice, 
            long dailyVolume)
        {
            context.Add(new ItemIndicator()
            {
                ItemId = id,
                JobRunId = jobRunId,
                BasePrice = basePrice,
                Count = count,
                TotalTrades = totalTrades,
                DailyVolume = dailyVolume
            });
        }

        private static void CreateItem(AppDbContext context, long id, string name)
        {
            if (context.Items.Find(id) == null)
                context.Add(new Item()
                {
                    Id = id,
                    Name = name,
                    RegistrationDate = DateTime.Now
                });
        }
    }
}
