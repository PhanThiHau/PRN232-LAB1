using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<Course?> GetCourseWithDetailsAsync(int courseId);
        IQueryable<Course> GetQueryable();
    }
}
