using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        public SemesterRepository(LmsDbContext context) : base(context)
        {
        }

        public async Task<Semester?> GetSemesterWithCoursesAsync(int semesterId)
        {
            return await _dbSet
                .Include(s => s.Courses)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SemesterId == semesterId);
        }

        public IQueryable<Semester> GetQueryable()
        {
            return _dbSet.AsNoTracking();
        }
    }
}
