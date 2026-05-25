using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Student?> GetStudentWithEnrollmentsAsync(int studentId);
        IQueryable<Student> GetQueryable();
    }
}
