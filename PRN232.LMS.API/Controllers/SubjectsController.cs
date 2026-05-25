using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>
    /// API Controller for managing Subject resources.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        /// <summary>
        /// Retrieves a paginated list of subjects with search, sort, paging, and field selection support.
        /// </summary>
        /// <param name="queryParams">Query parameters for filtering, sorting, paging, and field selection.</param>
        /// <returns>A paginated list of subjects with metadata.</returns>
        /// <response code="200">Returns the list of subjects.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSubjects([FromQuery] QueryParameters queryParams)
        {
            var result = await _subjectService.GetAllSubjectsAsync(queryParams);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific subject by ID.
        /// </summary>
        /// <param name="id">The subject ID.</param>
        /// <returns>The subject details.</returns>
        /// <response code="200">Returns the subject.</response>
        /// <response code="404">Subject not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var result = await _subjectService.GetSubjectByIdAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(result);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Creates a new subject.
        /// </summary>
        /// <param name="request">The subject creation request.</param>
        /// <returns>The newly created subject.</returns>
        /// <response code="201">Subject created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<SubjectResponse>.ErrorResponse("Validation failed.", errors));
            }

            var result = await _subjectService.CreateSubjectAsync(request);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Updates an existing subject by ID.
        /// </summary>
        /// <param name="id">The subject ID to update.</param>
        /// <param name="request">The subject update request.</param>
        /// <returns>The updated subject.</returns>
        /// <response code="200">Subject updated successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="404">Subject not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] UpdateSubjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<SubjectResponse>.ErrorResponse("Validation failed.", errors));
            }

            var result = await _subjectService.UpdateSubjectAsync(id, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(result);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Deletes a subject by ID.
        /// </summary>
        /// <param name="id">The subject ID to delete.</param>
        /// <returns>Deletion confirmation.</returns>
        /// <response code="200">Subject deleted successfully.</response>
        /// <response code="404">Subject not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var result = await _subjectService.DeleteSubjectAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(result);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }
    }
}
