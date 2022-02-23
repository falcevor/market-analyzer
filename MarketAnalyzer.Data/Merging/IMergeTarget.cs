using MarketAnalyzer.Core.Model;
using System.Threading.Tasks;

namespace MarketAnalyzer.Data.Merging
{
    public interface IMergeTarget
    {
        Task TruncateAsync();
        Task AddJobRunAsync(JobRun jobRun);
        Task AddItemAsync(Item item);
        Task AddItemIndicatorAsync(ItemIndicator itemIndicator);
        Task SaveChangesAsync();
    }
}