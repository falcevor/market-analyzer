using CsvHelper;
using MarketAnalyzer.Data.Model;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MarketAnalyzer.Data.Merging.Csv
{
    public class CsvMergeSource : IMergeSource
    {
        private byte[] _jobRunsFile;
        private byte[] _itemsFile;
        private byte[] _itemIndicatorsFile;

        public CsvMergeSource(byte[] jobRunsFile, byte[] itemsFile, byte[] itemIndicatorsFile)
        {
            _jobRunsFile = jobRunsFile;
            _itemsFile = itemsFile;
            _itemIndicatorsFile = itemIndicatorsFile;
        }

        public async Task<IEnumerable<JobRun>> GetJobRunsAsync()
        {
            using var reader = new StreamReader(new MemoryStream(_jobRunsFile), Encoding.UTF8);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecordsAsync<JobRunCsv>();

            var result = new List<JobRun>();
            await foreach (var record in records)
                result.Add(new JobRun() 
                {
                    Id = record.Id,
                    DetailedMessage = record.DetailedMessage,
                    Status = (JobStatus)record.Status,
                    RunDate = record.RunDate
                });

            return result;
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            using var reader = new StreamReader(new MemoryStream(_itemsFile), Encoding.UTF8);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecordsAsync<Item>();

            var result = new List<Item>();
            await foreach (var record in records)
                result.Add(new Item() 
                {
                    Id = record.Id,
                    Name = record.Name, 
                    RegistrationDate = record.RegistrationDate
                });

            return result;
        }

        public async Task<IEnumerable<ItemIndicator>> GetItemIndicatorsAsync()
        {
            using var reader = new StreamReader(new MemoryStream(_itemIndicatorsFile), Encoding.UTF8);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecordsAsync<ItemIndicator>();

            var result = new List<ItemIndicator>();
            await foreach (var record in records)
                result.Add(new ItemIndicator()
                {
                    Id = record.Id,
                    ItemId = record.ItemId,
                    JobRunId = record.JobRunId,
                    BasePrice = record.BasePrice,
                    Count = record.Count,
                    DailyVolume = record.DailyVolume,
                    TotalTrades = record.TotalTrades
                });

            return result;
        }

    }
}
