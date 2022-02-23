using MarketAnalyzer.Data;
using MarketAnalyzer.Data.Merging.Database;
using MarketAnalyzer.Core.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarketAnalyzer.UnitTests.Data.Merging
{
    public class DatabaseMergeTargetTests
    {
        [Fact]
        public async Task Should_call_job_run_add_on_context()
        {
            var contextMock = new Mock<AppDbContext>(new DbContextOptionsBuilder().Options);
            var target = new DatabaseMergeTarget(contextMock.Object);

            await target.AddJobRunAsync(new JobRun());

            contextMock.Verify(x => x.AddAsync(It.IsAny<JobRun>(), CancellationToken.None));
        }

        [Fact]
        public async Task Should_call_item_add_on_context()
        {
            var contextMock = new Mock<AppDbContext>(new DbContextOptionsBuilder().Options);
            var target = new DatabaseMergeTarget(contextMock.Object);

            await target.AddItemAsync(new Item());

            contextMock.Verify(x => x.AddAsync(It.IsAny<Item>(), CancellationToken.None));
        }

        [Fact]
        public async Task Should_call_item_indicator_add_on_context()
        {
            var contextMock = new Mock<AppDbContext>(new DbContextOptionsBuilder().Options);
            var target = new DatabaseMergeTarget(contextMock.Object);

            await target.AddItemIndicatorAsync(new ItemIndicator());

            contextMock.Verify(x => x.AddAsync(It.IsAny<ItemIndicator>(), CancellationToken.None));
        }

        [Fact]
        public async Task Should_call_save_changes_on_context()
        {
            var contextMock = new Mock<AppDbContext>(new DbContextOptionsBuilder().Options);
            var target = new DatabaseMergeTarget(contextMock.Object);

            await target.SaveChangesAsync();

            contextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None));
        }

        // Проверить вызов TruncateAsync() не можем, так как Migrate() у DatabaseFacade - метод расширения, который не мокается :(
        // TODO: Ввести новый уровень абстракции и тестить его
    }
}
