using MarketAnalyzer.Core.Abstraction;
using MarketAnalyzer.Core.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MarketAnalyzer.Data.Petsistence
{
    public class Store<T> : IStore<T> where T : Entity
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public Store(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IQueryable<T> Values 
        { 
            get 
            {
                var context = _contextFactory.CreateDbContext();
                var set = context.Set<T>();
                return set.AsNoTracking();
            }
        }

        public async Task Add(T item)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var set = context.Set<T>();
            await set.AddAsync(item);
            await context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var set = context.Set<T>();
            var item = await set.FindAsync(id);

            if (item is null) return;

            set.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task<T> Get(long id)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var set = context.Set<T>();
            var item = await set.FindAsync(id);
            return item;
        }

        public async Task Update(long id, T item)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var set = context.Set<T>();

            item.Id = id;
            set.Attach(item);
            set.Update(item);
            await context.SaveChangesAsync();
        }
    }
}
