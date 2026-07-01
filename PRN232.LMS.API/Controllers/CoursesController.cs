using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>
    /// API v1 Controller for managing Course resources.
    /// Supports nested resource access, search, sort, paging, field selection, and expansion.
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Retrieves a paginated list of courses with search, sort, paging, field selection, and expansion support.
        /// </summary>
        /// <param name="queryParams">Query parameters for filtering, sorting, paging, field selection, and expansion.</param>
        /// <returns>A paginated list of courses with metadata.</returns>
        /// <response code="200">Returns the list of courses.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCourses([FromQuery] QueryParameters queryParams)
        {
            var result = await _courseService.GetAllCoursesAsync(queryParams);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific course by ID, including related semester and enrollment data.
        /// </summary>
        /// <param name="id">The course ID.</param>
        /// <returns>The course with related details.</returns>
        /// <response code="200">Returns the course.</response>
        /// <response code="404">Course not found.</response>
        [HttpGet("{id:int}", Name = "GetCourseById")]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseById([FromRoute] int id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all students enrolled in a specific course. (Nested resource)
        /// </summary>
        /// <param name="courseId">The course ID.</param>
        /// <param name="queryParams">Query parameters for filtering, sorting, and paging.</param>
        /// <returns>Paginated list of enrollments for the course.</returns>
        /// <response code="200">Returns the list of enrollments.</response>
        /// <response code="404">Course not found.</response>
        [HttpGet("{courseId:int}/students")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentsByCourse([FromRoute] int courseId, [FromQuery] QueryParameters queryParams)
        {
            var result = await _courseService.GetCourseEnrollmentsAsync(courseId, queryParams);
            if (!result.Success)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Creates a new course. Requires authentication.
        /// </summary>
        /// <param name="request">The course creation request.</param>
        /// <returns>The newly created course.</returns>
        /// <response code="201">Course created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Unauthorized.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<CourseResponse>.ErrorResponse("Validation failed.", errors));
            }

            var result = await _courseService.CreateCourseAsync(request);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Updates an existing course by ID. Requires authentication.
        /// </summary>
        /// <param name="id">The course ID to update.</param>
        /// <param name="request">The course update request.</param>
        /// <returns>The updated course.</returns>
        /// <response code="200">Course updated successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="404">Course not found.</response>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCourse([FromRoute] int id, [FromBody] UpdateCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<CourseResponse>.ErrorResponse("Validation failed.", errors));
            }

            var result = await _courseService.UpdateCourseAsync(id, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Deletes a course by ID. Requires Admin role.
        /// </summary>
        /// <param name="id">The course ID to delete.</param>
        /// <returns>Deletion confirmation.</returns>
        /// <response code="200">Course deleted successfully.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden — Admin role required.</response>
        /// <response code="404">Course not found.</response>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of enrollments for a specific course.
        /// </summary>
        /// <param name="id">The course ID.</param>
        /// <param name="queryParams">Query parameters for filtering, sorting, paging, field selection, and expansion.</param>
        /// <returns>Paginated list of enrollments for the course.</returns>
        /// <response code="200">Returns the list of enrollments.</response>
        /// <response code="404">Course not found.</response>
        [HttpGet("{id:int}/enrollments")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseEnrollments([FromRoute] int id, [FromQuery] QueryParameters queryParams)
        {
            var result = await _courseService.GetCourseEnrollmentsAsync(id, queryParams);
            if (!result.Success)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }
    }
}
