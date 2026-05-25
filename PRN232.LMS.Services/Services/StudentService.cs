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
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedResult<object>>> GetAllStudentsAsync(QueryParameters queryParams)
        {
            try
            {
                IQueryable<Student> query = _studentRepository.GetQueryable();

                if (!string.IsNullOrWhiteSpace(queryParams.Search))
                {
                    var searchTerm = queryParams.Search.ToLower();
                    query = query.Where(s =>
                        s.FullName.ToLower().Contains(searchTerm) ||
                        s.Email.ToLower().Contains(searchTerm));
                }

                query = QueryHelper.ApplyExpansion(query, queryParams.Expand);
                query = QueryHelper.ApplySorting(query, queryParams.Sort);

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalItems / queryParams.Size);

                var students = await query
                    .Skip((queryParams.Page - 1) * queryParams.Size)
                    .Take(queryParams.Size)
                    .ToListAsync();

                var studentResponses = _mapper.Map<List<StudentResponse>>(students);
                var selectedData = QueryHelper.ApplyFieldSelection(studentResponses, queryParams.Fields);

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
                    "An error occurred while retrieving students.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<StudentResponse>> GetStudentByIdAsync(int id)
        {
            try
            {
                var student = await _studentRepository.GetStudentWithEnrollmentsAsync(id);

                if (student == null)
                {
                    return ApiResponse<StudentResponse>.ErrorResponse("Student not found.");
                }

                var response = _mapper.Map<StudentResponse>(student);
                return ApiResponse<StudentResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentResponse>.ErrorResponse(
                    "An error occurred while retrieving the student.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<StudentResponse>> CreateStudentAsync(CreateStudentRequest request)
        {
            try
            {
                var student = _mapper.Map<Student>(request);
                await _studentRepository.AddAsync(student);

                var response = _mapper.Map<StudentResponse>(student);
                return ApiResponse<StudentResponse>.SuccessResponse(response, "Student created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentResponse>.ErrorResponse(
                    "An error occurred while creating the student.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<StudentResponse>> UpdateStudentAsync(int id, UpdateStudentRequest request)
        {
            try
            {
                var existingStudent = await _studentRepository.GetByIdAsync(id);
                if (existingStudent == null)
                {
                    return ApiResponse<StudentResponse>.ErrorResponse("Student not found.");
                }

                _mapper.Map(request, existingStudent);
                existingStudent.StudentId = id;
                await _studentRepository.UpdateAsync(existingStudent);

                var response = _mapper.Map<StudentResponse>(existingStudent);
                return ApiResponse<StudentResponse>.SuccessResponse(response, "Student updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentResponse>.ErrorResponse(
                    "An error occurred while updating the student.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<object>> DeleteStudentAsync(int id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);
                if (student == null)
                {
                    return ApiResponse<object>.ErrorResponse("Student not found.");
                }

                await _studentRepository.DeleteAsync(student);
                return ApiResponse<object>.SuccessResponse(null!, "Student deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse(
                    "An error occurred while deleting the student.",
                    new List<string> { ex.Message });
            }
        }
    }
}
