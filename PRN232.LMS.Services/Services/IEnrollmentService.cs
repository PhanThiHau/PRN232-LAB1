using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;

namespace PRN232.LMS.Services.Services
{
    public interface IEnrollmentService
    {
        Task<ApiResponse<PagedResult<object>>> GetAllEnrollmentsAsync(QueryParameters queryParams);
        Task<ApiResponse<EnrollmentResponse>> GetEnrollmentByIdAsync(int id);
        Task<ApiResponse<EnrollmentResponse>> CreateEnrollmentAsync(CreateEnrollmentRequest request);
        Task<ApiResponse<EnrollmentResponse>> UpdateEnrollmentAsync(int id, UpdateEnrollmentRequest request);
        Task<ApiResponse<object>> DeleteEnrollmentAsync(int id);
    }
}
