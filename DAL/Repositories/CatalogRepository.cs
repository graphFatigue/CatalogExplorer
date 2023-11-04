using Core.Entity;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CatalogRepository : IBaseRepository<Catalog>
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<Catalog> _dbSet;

        public CatalogRepository(
            AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Catalog>();
        }

        public async Task CreateAsync(Catalog entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsyncRecursive(Catalog entity)
        {
            if (entity.ChildrenCatalogs.Any())
            {
                foreach (var child in entity.ChildrenCatalogs)
                {
                    await DeleteAsyncRecursive(child);
                }
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

    public async Task<IEnumerable<Catalog>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Catalog?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Catalog> UpdateAsync(Catalog entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
