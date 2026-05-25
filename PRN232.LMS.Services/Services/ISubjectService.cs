using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;

namespace PRN232.LMS.Services.Services
{
    public interface ISubjectService
    {
        Task<ApiResponse<PagedResult<object>>> GetAllSubjectsAsync(QueryParameters queryParams);
        Task<ApiResponse<SubjectResponse>> GetSubjectByIdAsync(int id);
        Task<ApiResponse<SubjectResponse>> CreateSubjectAsync(CreateSubjectRequest request);
        Task<ApiResponse<SubjectResponse>> UpdateSubjectAsync(int id, UpdateSubjectRequest request);
        Task<ApiResponse<object>> DeleteSubjectAsync(int id);
    }
}
