using MarketAnalyzer.Core.Abstraction;
using MarketAnalyzer.Core.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketAnalyzer.Data.Petsistence
{
    public class Store<T> : IStore<T> where T : Entity
    {
        private readonly AppDbContext _context;

        public Store(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            var set = _context.Set<T>();
            return set.AsNoTracking();
        }

        public async Task Add(T item)
        {
            var set = _context.Set<T>();
            await set.AddAsync(item);
        }

        public async Task Delete(long id)
        {
            var set = _context.Set<T>();
            var item = await set.FindAsync(id);

            if (item is null) return;

            set.Remove(item);
        }

        public async Task<T> Get(long id)
        {
            var set = _context.Set<T>();
            var item = await set.FindAsync(id);
            return item;
        }

        public Task Update(long id, T item)
        {
            var set = _context.Set<T>();

            item.Id = id;
            set.Attach(item);
            set.Update(item);

            return Task.CompletedTask;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

    }
}
