using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(LmsDbContext context) : base(context)
        {
        }

        public async Task<Student?> GetStudentWithEnrollmentsAsync(int studentId)
        {
            return await _dbSet
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Semester)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public IQueryable<Student> GetQueryable()
        {
            return _dbSet.AsNoTracking();
        }
    }
}
