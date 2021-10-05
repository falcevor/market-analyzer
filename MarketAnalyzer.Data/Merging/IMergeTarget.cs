using MarketAnalyzer.Data.Model;
using System.Threading.Tasks;

namespace MarketAnalyzer.Data.Merging
{
    public interface IMergeTarget
    {
        Task TruncateAsync();
        Task AddJobRunAsync(JobRun jobRun);
        Task AddItemAsync(Item jobRun);
        Task AddItemIndicatorAsync(ItemIndicator jobRun);
        Task SaveChangesAsync();
    }
}