using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(LmsDbContext context) : base(context)
        {
        }

        public async Task<Course?> GetCourseWithDetailsAsync(int courseId)
        {
            return await _dbSet
                .Include(c => c.Semester)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public IQueryable<Course> GetQueryable()
        {
            return _dbSet.AsNoTracking();
        }
    }
}
