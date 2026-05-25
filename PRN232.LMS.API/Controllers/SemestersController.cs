using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>
    /// API Controller for managing Semester resources.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemestersController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        /// <summary>
        /// Retrieves a paginated list of semesters with search, sort, paging, field selection, and expansion support.
        /// </summary>
        /// <param name="queryParams">Query parameters for filtering, sorting, paging, field selection, and expansion.</param>
        /// <returns>A paginated list of semesters with metadata.</returns>
        /// <response code="200">Returns the list of semesters.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSemesters([FromQuery] QueryParameters queryParams)
        {
            var result = await _semesterService.GetAllSemestersAsync(queryParams);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific semester by ID, including related courses.
        /// </summary>
        /// <param name="id">The semester ID.</param>
        /// <returns>The semester with course details.</returns>
        /// <response code="200">Returns the semester.</response>
        /// <response code="404">Semester not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSemesterById(int id)
        {
            var result = await _semesterService.GetSemesterByIdAsync(id);
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
        /// Creates a new semester.
        /// </summary>
        /// <param name="request">The semester creation request.</param>
        /// <returns>The newly created semester.</returns>
        /// <response code="201">Semester created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSemester([FromBody] CreateSemesterRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<SemesterResponse>.ErrorResponse("Validation failed.", errors));
            }

            var result = await _semesterService.CreateSemesterAsync(request);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Updates an existing semester by ID.
        /// </summary>
        /// <param name="id">The semester ID to update.</param>
        /// <param name="request">The semester update request.</param>
        /// <returns>The updated semester.</returns>
        /// <response code="200">Semester updated successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="404">Semester not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSemester(int id, [FromBody] UpdateSemesterRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<SemesterResponse>.ErrorResponse("Validation failed.", errors));
            }

            var result = await _semesterService.UpdateSemesterAsync(id, request);
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
        /// Deletes a semester by ID.
        /// </summary>
        /// <param name="id">The semester ID to delete.</param>
        /// <returns>Deletion confirmation.</returns>
        /// <response code="200">Semester deleted successfully.</response>
        /// <response code="404">Semester not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSemester(int id)
        {
            var result = await _semesterService.DeleteSemesterAsync(id);
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
