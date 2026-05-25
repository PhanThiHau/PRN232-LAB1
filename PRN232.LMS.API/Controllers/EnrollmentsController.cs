using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>
    /// API Controller for managing Enrollment resources.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        /// <summary>
        /// Retrieves a paginated list of enrollments with search, sort, paging, field selection, and expansion support.
        /// </summary>
        /// <param name="queryParams">Query parameters for filtering, sorting, paging, field selection, and expansion.</param>
        /// <returns>A paginated list of enrollments with metadata.</returns>
        /// <response code="200">Returns the list of enrollments.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllEnrollments([FromQuery] QueryParameters queryParams)
        {
            var result = await _enrollmentService.GetAllEnrollmentsAsync(queryParams);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific enrollment by ID, including related student and course data.
        /// </summary>
        /// <param name="id">The enrollment ID.</param>
        /// <returns>The enrollment with related details.</returns>
        /// <response code="200">Returns the enrollment.</response>
        /// <response code="404">Enrollment not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEnrollmentById(int id)
        {
            var result = await _enrollmentService.GetEnrollmentByIdAsync(id);
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
        /// Creates a new enrollment.
        /// </summary>
        /// <param name="request">The enrollment creation request.</param>
        /// <returns>The newly created enrollment.</returns>
        /// <response code="201">Enrollment created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<EnrollmentResponse>.ErrorResponse("Validation failed.", errors));
            }

            var result = await _enrollmentService.CreateEnrollmentAsync(request);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Updates an existing enrollment by ID.
        /// </summary>
        /// <param name="id">The enrollment ID to update.</param>
        /// <param name="request">The enrollment update request.</param>
        /// <returns>The updated enrollment.</returns>
        /// <response code="200">Enrollment updated successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="404">Enrollment not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] UpdateEnrollmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<EnrollmentResponse>.ErrorResponse("Validation failed.", errors));
            }

            var result = await _enrollmentService.UpdateEnrollmentAsync(id, request);
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
        /// Deletes an enrollment by ID.
        /// </summary>
        /// <param name="id">The enrollment ID to delete.</param>
        /// <returns>Deletion confirmation.</returns>
        /// <response code="200">Enrollment deleted successfully.</response>
        /// <response code="404">Enrollment not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var result = await _enrollmentService.DeleteEnrollmentAsync(id);
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
