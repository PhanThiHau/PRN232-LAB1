using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers
{
    /// <summary>
    /// API Controller for managing Student resources.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Retrieves a paginated list of students with search, sort, paging, field selection, and expansion support.
        /// </summary>
        /// <param name="queryParams">Query parameters for filtering, sorting, paging, field selection, and expansion.</param>
        /// <returns>A paginated list of students with metadata.</returns>
        /// <response code="200">Returns the list of students.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllStudents([FromQuery] QueryParameters queryParams)
        {
            var result = await _studentService.GetAllStudentsAsync(queryParams);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific student by ID, including related enrollment data.
        /// </summary>
        /// <param name="id">The student ID.</param>
        /// <returns>The student with enrollment details.</returns>
        /// <response code="200">Returns the student.</response>
        /// <response code="404">Student not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var result = await _studentService.GetStudentByIdAsync(id);
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
        /// Creates a new student.
        /// </summary>
        /// <param name="request">The student creation request.</param>
        /// <returns>The newly created student.</returns>
        /// <response code="201">Student created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<StudentResponse>.ErrorResponse("Validation failed.", errors));
            }

            var result = await _studentService.CreateStudentAsync(request);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Updates an existing student by ID.
        /// </summary>
        /// <param name="id">The student ID to update.</param>
        /// <param name="request">The student update request.</param>
        /// <returns>The updated student.</returns>
        /// <response code="200">Student updated successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="404">Student not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<StudentResponse>.ErrorResponse("Validation failed.", errors));
            }

            var result = await _studentService.UpdateStudentAsync(id, request);
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
        /// Deletes a student by ID.
        /// </summary>
        /// <param name="id">The student ID to delete.</param>
        /// <returns>Deletion confirmation.</returns>
        /// <response code="200">Student deleted successfully.</response>
        /// <response code="404">Student not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
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
