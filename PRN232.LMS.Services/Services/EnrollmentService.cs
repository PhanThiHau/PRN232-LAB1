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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IMapper _mapper;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, IMapper mapper)
        {
            _enrollmentRepository = enrollmentRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedResult<object>>> GetAllEnrollmentsAsync(QueryParameters queryParams)
        {
            try
            {
                IQueryable<Enrollment> query = _enrollmentRepository.GetQueryable();

                if (!string.IsNullOrWhiteSpace(queryParams.Search))
                {
                    var searchTerm = queryParams.Search.ToLower();
                    query = query.Where(e =>
                        e.Status.ToLower().Contains(searchTerm) ||
                        e.Student.FullName.ToLower().Contains(searchTerm) ||
                        e.Course.CourseName.ToLower().Contains(searchTerm));
                }

                query = QueryHelper.ApplyExpansion(query, queryParams.Expand);
                query = QueryHelper.ApplySorting(query, queryParams.Sort);

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalItems / queryParams.Size);

                var enrollments = await query
                    .Skip((queryParams.Page - 1) * queryParams.Size)
                    .Take(queryParams.Size)
                    .ToListAsync();

                var enrollmentResponses = _mapper.Map<List<EnrollmentResponse>>(enrollments);
                var selectedData = QueryHelper.ApplyFieldSelection(enrollmentResponses, queryParams.Fields);

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
                    "An error occurred while retrieving enrollments.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<EnrollmentResponse>> GetEnrollmentByIdAsync(int id)
        {
            try
            {
                var enrollment = await _enrollmentRepository.GetEnrollmentWithDetailsAsync(id);

                if (enrollment == null)
                {
                    return ApiResponse<EnrollmentResponse>.ErrorResponse("Enrollment not found.");
                }

                var response = _mapper.Map<EnrollmentResponse>(enrollment);
                return ApiResponse<EnrollmentResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<EnrollmentResponse>.ErrorResponse(
                    "An error occurred while retrieving the enrollment.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<EnrollmentResponse>> CreateEnrollmentAsync(CreateEnrollmentRequest request)
        {
            try
            {
                var enrollment = _mapper.Map<Enrollment>(request);
                await _enrollmentRepository.AddAsync(enrollment);

                var response = _mapper.Map<EnrollmentResponse>(enrollment);
                return ApiResponse<EnrollmentResponse>.SuccessResponse(response, "Enrollment created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<EnrollmentResponse>.ErrorResponse(
                    "An error occurred while creating the enrollment.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<EnrollmentResponse>> UpdateEnrollmentAsync(int id, UpdateEnrollmentRequest request)
        {
            try
            {
                var existingEnrollment = await _enrollmentRepository.GetByIdAsync(id);
                if (existingEnrollment == null)
                {
                    return ApiResponse<EnrollmentResponse>.ErrorResponse("Enrollment not found.");
                }

                _mapper.Map(request, existingEnrollment);
                existingEnrollment.EnrollmentId = id;
                await _enrollmentRepository.UpdateAsync(existingEnrollment);

                var response = _mapper.Map<EnrollmentResponse>(existingEnrollment);
                return ApiResponse<EnrollmentResponse>.SuccessResponse(response, "Enrollment updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<EnrollmentResponse>.ErrorResponse(
                    "An error occurred while updating the enrollment.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<object>> DeleteEnrollmentAsync(int id)
        {
            try
            {
                var enrollment = await _enrollmentRepository.GetByIdAsync(id);
                if (enrollment == null)
                {
                    return ApiResponse<object>.ErrorResponse("Enrollment not found.");
                }

                await _enrollmentRepository.DeleteAsync(enrollment);
                return ApiResponse<object>.SuccessResponse(null!, "Enrollment deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse(
                    "An error occurred while deleting the enrollment.",
                    new List<string> { ex.Message });
            }
        }
    }
}
