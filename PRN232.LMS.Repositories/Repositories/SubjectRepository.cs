using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(LmsDbContext context) : base(context)
        {
        }

        public IQueryable<Subject> GetQueryable()
        {
            return _dbSet.AsNoTracking();
        }
    }
}
