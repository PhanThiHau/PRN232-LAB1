using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;

namespace PRN232.LMS.Services.Services
{
    public interface IStudentService
    {
        Task<ApiResponse<PagedResult<object>>> GetAllStudentsAsync(QueryParameters queryParams);
        Task<ApiResponse<StudentResponse>> GetStudentByIdAsync(int id);
        Task<ApiResponse<StudentResponse>> CreateStudentAsync(CreateStudentRequest request);
        Task<ApiResponse<StudentResponse>> UpdateStudentAsync(int id, UpdateStudentRequest request);
        Task<ApiResponse<object>> DeleteStudentAsync(int id);
    }
}
