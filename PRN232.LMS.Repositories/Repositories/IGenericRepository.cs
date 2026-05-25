using System.Linq.Expressions;

namespace PRN232.LMS.Repositories.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync(IQueryable<TEntity> query);
    }
}
