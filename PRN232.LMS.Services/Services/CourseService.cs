using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Repositories;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Helpers;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;

namespace PRN232.LMS.Services.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedResult<object>>> GetAllCoursesAsync(QueryParameters queryParams)
        {
            try
            {
                IQueryable<Course> query = _courseRepository.GetQueryable();

                if (!string.IsNullOrWhiteSpace(queryParams.Search))
                {
                    var searchTerm = queryParams.Search.ToLower();
                    query = query.Where(c => c.CourseName.ToLower().Contains(searchTerm));
                }

                query = QueryHelper.ApplyExpansion(query, queryParams.Expand);
                query = QueryHelper.ApplySorting(query, queryParams.Sort);

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalItems / queryParams.Size);

                var courses = await query
                    .Skip((queryParams.Page - 1) * queryParams.Size)
                    .Take(queryParams.Size)
                    .ToListAsync();

                var courseResponses = _mapper.Map<List<CourseResponse>>(courses);
                var selectedData = QueryHelper.ApplyFieldSelection(courseResponses, queryParams.Fields);

                var pagedResult = new PagedResult<object>
                {
                    Items = selectedData,
                    Pagination = new PaginationMetadata
                    {
                        Page = queryParams.Page,
                        PageSize = queryParams.Size,
                        TotalItems = totalItems,
                        TotalPages = totalPages
                    }
                };

                return ApiResponse<PagedResult<object>>.SuccessResponse(pagedResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<object>>.ErrorResponse(
                    "An error occurred while retrieving courses.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<CourseResponse>> GetCourseByIdAsync(int id)
        {
            try
            {
                var course = await _courseRepository.GetCourseWithDetailsAsync(id);
                if (course == null)
                {
                    return ApiResponse<CourseResponse>.ErrorResponse("Course not found.");
                }

                var response = _mapper.Map<CourseResponse>(course);
                return ApiResponse<CourseResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseResponse>.ErrorResponse(
                    "An error occurred while retrieving the course.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<CourseResponse>> CreateCourseAsync(CreateCourseRequest request)
        {
            try
            {
                var course = _mapper.Map<Course>(request);
                await _courseRepository.AddAsync(course);

                var response = _mapper.Map<CourseResponse>(course);
                return ApiResponse<CourseResponse>.SuccessResponse(response, "Course created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseResponse>.ErrorResponse(
                    "An error occurred while creating the course.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<CourseResponse>> UpdateCourseAsync(int id, UpdateCourseRequest request)
        {
            try
            {
                var existingCourse = await _courseRepository.GetByIdAsync(id);
                if (existingCourse == null)
                {
                    return ApiResponse<CourseResponse>.ErrorResponse("Course not found.");
                }

                _mapper.Map(request, existingCourse);
                existingCourse.CourseId = id;
                await _courseRepository.UpdateAsync(existingCourse);

                var response = _mapper.Map<CourseResponse>(existingCourse);
                return ApiResponse<CourseResponse>.SuccessResponse(response, "Course updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseResponse>.ErrorResponse(
                    "An error occurred while updating the course.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<object>> DeleteCourseAsync(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);
                if (course == null)
                {
                    return ApiResponse<object>.ErrorResponse("Course not found.");
                }

                await _courseRepository.DeleteAsync(course);
                return ApiResponse<object>.SuccessResponse(null!, "Course deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse(
                    "An error occurred while deleting the course.",
                    new List<string> { ex.Message });
            }
        }
    }
}
