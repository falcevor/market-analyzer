using FluentAssertions;
using MarketAnalyzer.Data.Merging.Csv;
using MarketAnalyzer.Data.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MarketAnalyzer.UnitTests.Data.Merging
{
    public class CsvMergeSourceTests
    {
        [Fact]
        public async Task Should_return_empty_with_empty_files()
        {
            var source = new CsvMergeSource(new byte[0], new byte[0], new byte[0]);
            (await source.GetJobRunsAsync()).Should().BeEmpty();
            (await source.GetItemsAsync()).Should().BeEmpty();
            (await source.GetItemIndicatorsAsync()).Should().BeEmpty();
        }

        [Fact]
        public async Task Should_return_correct_items_count()
        {
            var source = new CsvMergeSource(GetTestJobRunsCsv(), GetTestItemsCsv(), GetTestItemIndicatorsCsv());
            (await source.GetJobRunsAsync()).Should().HaveCount(4);
            (await source.GetItemsAsync()).Should().HaveCount(4);
            (await source.GetItemIndicatorsAsync()).Should().HaveCount(4);
        }

        [Fact]
        public async Task Should_return_correct_initialized_JobRuns()
        {
            var source = new CsvMergeSource(GetTestJobRunsCsv(), GetTestItemsCsv(), GetTestItemIndicatorsCsv());
            var jobRuns = (await source.GetJobRunsAsync()).ToArray();

            jobRuns[0].Id.Should().Be(1);
            jobRuns[0].RunDate.Should()
                .HaveYear(2021)
                .And.HaveMonth(9)
                .And.HaveDay(23);
            jobRuns[0].Status.Should().Be(JobStatus.Success);
            jobRuns[2].Status.Should().Be(JobStatus.Failure);
            jobRuns[2].DetailedMessage.Should().StartWith("Resource temporarily unavailable");
        }

        [Fact]
        public async Task Should_return_correct_initialized_Items()
        {
            var source = new CsvMergeSource(GetTestJobRunsCsv(), GetTestItemsCsv(), GetTestItemIndicatorsCsv());
            var jobRuns = (await source.GetItemsAsync()).ToArray();

            jobRuns[0].Id.Should().Be(46798);
            jobRuns[0].Name.Should().Be("(Валенсия) Истинно-бирюзовый");
            jobRuns[0].RegistrationDate.Should()
                .HaveYear(2021)
                .And.HaveMonth(9)
                .And.HaveDay(23);
        }

        [Fact]
        public async Task Should_return_correct_initialized_ItemIndicators()
        {
            var source = new CsvMergeSource(GetTestJobRunsCsv(), GetTestItemsCsv(), GetTestItemIndicatorsCsv());
            var jobRuns = (await source.GetItemIndicatorsAsync()).ToArray();

            jobRuns[0].Id.Should().Be(1);
            jobRuns[0].ItemId.Should().Be(46798);
            jobRuns[0].JobRunId.Should().Be(1);
            jobRuns[0].Count.Should().Be(0);
            jobRuns[0].TotalTrades.Should().Be(110);
            jobRuns[0].BasePrice.Should().Be(46600000);
            jobRuns[0].DailyVolume.Should().Be(1);
        }

        private byte[] GetTestJobRunsCsv()
        {
            return Encoding.UTF8.GetBytes(
                @"Id,RunDate,Status,DetailedMessage
1,2021-09-23 06:56:51.640014,1,
2,2021-09-23 06:58:25.157378,1,
3,2021-09-23 07:01:34.238232,2,Resource temporarily unavailable (oauth2.googleapis.com:443)
4,2021-09-23 07:04:07.575381,1,"
            );
        }

        private byte[] GetTestItemsCsv()
        {
            return Encoding.UTF8.GetBytes(
                @"Id,Name,RegistrationDate
46798,(Валенсия) Истинно-бирюзовый,2021-09-23 06:56:53.656284
13124,Роговой лук Креи,2021-09-23 06:56:55.185526
13140,Роговой лук Кутума,2021-09-23 06:56:55.186116
13138,Роговой лук Нубэра,2021-09-23 06:56:55.186709"
            );
        }

        private byte[] GetTestItemIndicatorsCsv()
        {
            return Encoding.UTF8.GetBytes(
                @"Id,ItemId,JobRunId,Count,TotalTrades,BasePrice,DailyVolume
1,46798,1,0,110,46600000,1
2,9735,1,0,106373,277000,28
3,13124,1,1,314384,28800,44
4,13140,1,40,16279,155000000,6"
            );
        }
    }
}
