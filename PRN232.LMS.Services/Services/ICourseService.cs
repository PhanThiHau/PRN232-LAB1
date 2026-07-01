using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;

namespace PRN232.LMS.Services.Services
{
    public interface ICourseService
    {
        Task<ApiResponse<PagedResult<object>>> GetAllCoursesAsync(QueryParameters queryParams);
        Task<ApiResponse<CourseResponse>> GetCourseByIdAsync(int id);
        Task<ApiResponse<CourseResponse>> CreateCourseAsync(CreateCourseRequest request);
        Task<ApiResponse<CourseResponse>> UpdateCourseAsync(int id, UpdateCourseRequest request);
        Task<ApiResponse<object>> DeleteCourseAsync(int id);
        Task<ApiResponse<PagedResult<object>>> GetCourseEnrollmentsAsync(int courseId, QueryParameters queryParams);
    }
}
