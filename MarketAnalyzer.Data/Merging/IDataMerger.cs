using System.Threading.Tasks;

namespace MarketAnalyzer.Data.Merging
{
    public interface IDataMerger
    {
        Task MergeAsync(IMergeSource source, IMergeTarget target);
    }
}
