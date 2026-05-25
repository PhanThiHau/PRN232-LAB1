using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories
{
    public interface ISemesterRepository : IGenericRepository<Semester>
    {
        Task<Semester?> GetSemesterWithCoursesAsync(int semesterId);
        IQueryable<Semester> GetQueryable();
    }
}
