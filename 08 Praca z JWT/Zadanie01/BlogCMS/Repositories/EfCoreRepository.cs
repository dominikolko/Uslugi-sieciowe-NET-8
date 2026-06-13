using Microsoft.EntityFrameworkCore;
using BlogCMS.Data;

namespace BlogCMS.Repositories
{
    public class EfCoreRepository<T> : IRepository<T>
        where T : class
    {
        private readonly BlogDbContext _context;
        private readonly DbSet<T> _entities;

        public EfCoreRepository(BlogDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<int> AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            var idProperty = entity.GetType().GetProperty("Id");

            return (int)(idProperty?.GetValue(entity) ?? 0);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _entities.Update(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _entities.FindAsync(id);

            if (entity == null)
                return false;

            _entities.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}