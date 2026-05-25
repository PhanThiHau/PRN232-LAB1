using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task<Enrollment?> GetEnrollmentWithDetailsAsync(int enrollmentId);
        IQueryable<Enrollment> GetQueryable();
    }
}
