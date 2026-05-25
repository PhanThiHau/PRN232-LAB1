using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        IQueryable<Subject> GetQueryable();
    }
}
