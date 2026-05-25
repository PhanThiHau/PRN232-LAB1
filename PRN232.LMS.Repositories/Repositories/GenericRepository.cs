using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;

namespace PRN232.LMS.Repositories.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly LmsDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(LmsDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public Task<IQueryable<TEntity>> GetAllAsync()
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            return Task.FromResult(query);
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity != null;
        }

        public async Task<int> CountAsync(IQueryable<TEntity> query)
        {
            return await query.CountAsync();
        }
    }
}
