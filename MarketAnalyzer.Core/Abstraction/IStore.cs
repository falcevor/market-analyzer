using MarketAnalyzer.Core.Model;

namespace MarketAnalyzer.Core.Abstraction
{
    public interface IStore<T> where T : Entity
    {
        IQueryable<T> Values { get; }
        Task<T> Get(long id);
        Task Delete(long id);
        Task Add(T item);
        Task Update(long id, T item);
    }
}
