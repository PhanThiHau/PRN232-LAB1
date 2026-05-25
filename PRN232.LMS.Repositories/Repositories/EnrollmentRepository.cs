using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(LmsDbContext context) : base(context)
        {
        }

        public async Task<Enrollment?> GetEnrollmentWithDetailsAsync(int enrollmentId)
        {
            return await _dbSet
                .Include(e => e.Student)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Semester)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
        }

        public IQueryable<Enrollment> GetQueryable()
        {
            return _dbSet.AsNoTracking();
        }
    }
}
