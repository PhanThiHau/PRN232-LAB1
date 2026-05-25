using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;

namespace PRN232.LMS.Services.Services
{
    public interface ISemesterService
    {
        Task<ApiResponse<PagedResult<object>>> GetAllSemestersAsync(QueryParameters queryParams);
        Task<ApiResponse<SemesterResponse>> GetSemesterByIdAsync(int id);
        Task<ApiResponse<SemesterResponse>> CreateSemesterAsync(CreateSemesterRequest request);
        Task<ApiResponse<SemesterResponse>> UpdateSemesterAsync(int id, UpdateSemesterRequest request);
        Task<ApiResponse<object>> DeleteSemesterAsync(int id);
    }
}
