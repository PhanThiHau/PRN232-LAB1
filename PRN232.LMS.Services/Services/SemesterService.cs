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
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IMapper _mapper;

        public SemesterService(ISemesterRepository semesterRepository, IMapper mapper)
        {
            _semesterRepository = semesterRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedResult<object>>> GetAllSemestersAsync(QueryParameters queryParams)
        {
            try
            {
                IQueryable<Semester> query = _semesterRepository.GetQueryable();

                if (!string.IsNullOrWhiteSpace(queryParams.Search))
                {
                    var searchTerm = queryParams.Search.ToLower();
                    query = query.Where(s => s.SemesterName.ToLower().Contains(searchTerm));
                }

                query = QueryHelper.ApplyExpansion(query, queryParams.Expand);
                query = QueryHelper.ApplySorting(query, queryParams.Sort);

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalItems / queryParams.Size);

                var semesters = await query
                    .Skip((queryParams.Page - 1) * queryParams.Size)
                    .Take(queryParams.Size)
                    .ToListAsync();

                var semesterResponses = _mapper.Map<List<SemesterResponse>>(semesters);
                var selectedData = QueryHelper.ApplyFieldSelection(semesterResponses, queryParams.Fields);

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
                    "An error occurred while retrieving semesters.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<SemesterResponse>> GetSemesterByIdAsync(int id)
        {
            try
            {
                var semester = await _semesterRepository.GetSemesterWithCoursesAsync(id);
                if (semester == null)
                {
                    return ApiResponse<SemesterResponse>.ErrorResponse("Semester not found.");
                }

                var response = _mapper.Map<SemesterResponse>(semester);
                return ApiResponse<SemesterResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<SemesterResponse>.ErrorResponse(
                    "An error occurred while retrieving the semester.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<SemesterResponse>> CreateSemesterAsync(CreateSemesterRequest request)
        {
            try
            {
                var semester = _mapper.Map<Semester>(request);
                await _semesterRepository.AddAsync(semester);

                var response = _mapper.Map<SemesterResponse>(semester);
                return ApiResponse<SemesterResponse>.SuccessResponse(response, "Semester created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<SemesterResponse>.ErrorResponse(
                    "An error occurred while creating the semester.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<SemesterResponse>> UpdateSemesterAsync(int id, UpdateSemesterRequest request)
        {
            try
            {
                var existingSemester = await _semesterRepository.GetByIdAsync(id);
                if (existingSemester == null)
                {
                    return ApiResponse<SemesterResponse>.ErrorResponse("Semester not found.");
                }

                _mapper.Map(request, existingSemester);
                existingSemester.SemesterId = id;
                await _semesterRepository.UpdateAsync(existingSemester);

                var response = _mapper.Map<SemesterResponse>(existingSemester);
                return ApiResponse<SemesterResponse>.SuccessResponse(response, "Semester updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<SemesterResponse>.ErrorResponse(
                    "An error occurred while updating the semester.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<object>> DeleteSemesterAsync(int id)
        {
            try
            {
                var semester = await _semesterRepository.GetByIdAsync(id);
                if (semester == null)
                {
                    return ApiResponse<object>.ErrorResponse("Semester not found.");
                }

                await _semesterRepository.DeleteAsync(semester);
                return ApiResponse<object>.SuccessResponse(null!, "Semester deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse(
                    "An error occurred while deleting the semester.",
                    new List<string> { ex.Message });
            }
        }
    }
}
