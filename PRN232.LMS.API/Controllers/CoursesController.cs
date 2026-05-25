using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>
    /// API Controller for managing Course resources.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
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
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);
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
        /// Creates a new course.
        /// </summary>
        /// <param name="request">The course creation request.</param>
        /// <returns>The newly created course.</returns>
        /// <response code="201">Course created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status500InternalServerError)]
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
        /// Updates an existing course by ID.
        /// </summary>
        /// <param name="id">The course ID to update.</param>
        /// <param name="request">The course update request.</param>
        /// <returns>The updated course.</returns>
        /// <response code="200">Course updated successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="404">Course not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseRequest request)
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
                {
                    return NotFound(result);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Deletes a course by ID.
        /// </summary>
        /// <param name="id">The course ID to delete.</param>
        /// <returns>Deletion confirmation.</returns>
        /// <response code="200">Course deleted successfully.</response>
        /// <response code="404">Course not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
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
