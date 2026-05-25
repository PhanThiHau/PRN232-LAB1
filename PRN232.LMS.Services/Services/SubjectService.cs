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
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;

        public SubjectService(ISubjectRepository subjectRepository, IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedResult<object>>> GetAllSubjectsAsync(QueryParameters queryParams)
        {
            try
            {
                IQueryable<Subject> query = _subjectRepository.GetQueryable();

                if (!string.IsNullOrWhiteSpace(queryParams.Search))
                {
                    var searchTerm = queryParams.Search.ToLower();
                    query = query.Where(s =>
                        s.SubjectCode.ToLower().Contains(searchTerm) ||
                        s.SubjectName.ToLower().Contains(searchTerm));
                }

                query = QueryHelper.ApplySorting(query, queryParams.Sort);

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalItems / queryParams.Size);

                var subjects = await query
                    .Skip((queryParams.Page - 1) * queryParams.Size)
                    .Take(queryParams.Size)
                    .ToListAsync();

                var subjectResponses = _mapper.Map<List<SubjectResponse>>(subjects);
                var selectedData = QueryHelper.ApplyFieldSelection(subjectResponses, queryParams.Fields);

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
                    "An error occurred while retrieving subjects.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<SubjectResponse>> GetSubjectByIdAsync(int id)
        {
            try
            {
                var subject = await _subjectRepository.GetByIdAsync(id);
                if (subject == null)
                {
                    return ApiResponse<SubjectResponse>.ErrorResponse("Subject not found.");
                }

                var response = _mapper.Map<SubjectResponse>(subject);
                return ApiResponse<SubjectResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<SubjectResponse>.ErrorResponse(
                    "An error occurred while retrieving the subject.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<SubjectResponse>> CreateSubjectAsync(CreateSubjectRequest request)
        {
            try
            {
                var subject = _mapper.Map<Subject>(request);
                await _subjectRepository.AddAsync(subject);

                var response = _mapper.Map<SubjectResponse>(subject);
                return ApiResponse<SubjectResponse>.SuccessResponse(response, "Subject created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<SubjectResponse>.ErrorResponse(
                    "An error occurred while creating the subject.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<SubjectResponse>> UpdateSubjectAsync(int id, UpdateSubjectRequest request)
        {
            try
            {
                var existingSubject = await _subjectRepository.GetByIdAsync(id);
                if (existingSubject == null)
                {
                    return ApiResponse<SubjectResponse>.ErrorResponse("Subject not found.");
                }

                _mapper.Map(request, existingSubject);
                existingSubject.SubjectId = id;
                await _subjectRepository.UpdateAsync(existingSubject);

                var response = _mapper.Map<SubjectResponse>(existingSubject);
                return ApiResponse<SubjectResponse>.SuccessResponse(response, "Subject updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<SubjectResponse>.ErrorResponse(
                    "An error occurred while updating the subject.",
                    new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<object>> DeleteSubjectAsync(int id)
        {
            try
            {
                var subject = await _subjectRepository.GetByIdAsync(id);
                if (subject == null)
                {
                    return ApiResponse<object>.ErrorResponse("Subject not found.");
                }

                await _subjectRepository.DeleteAsync(subject);
                return ApiResponse<object>.SuccessResponse(null!, "Subject deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse(
                    "An error occurred while deleting the subject.",
                    new List<string> { ex.Message });
            }
        }
    }
}
