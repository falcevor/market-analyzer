using MarketAnalyzer.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketAnalyzer.Data.Merging
{
    public interface IMergeSource
    {
        Task<IEnumerable<JobRun>> GetJobRunsAsync();
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<IEnumerable<ItemIndicator>> GetItemIndicatorsAsync();
    }
}