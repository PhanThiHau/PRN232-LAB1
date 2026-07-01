using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers.V2
{
    /// <summary>
    /// API v2 Controller for managing Student resources.
    /// Enhanced version with additional computed fields (age, enrollmentCount).
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/students")]
    public class StudentsV2Controller : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsV2Controller(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// [v2] Retrieves a paginated list of students with enhanced metadata including computed age.
        /// </summary>
        /// <param name="queryParams">Query parameters for filtering, sorting, and paging.</param>
        /// <param name="xRequestId">Optional request tracking ID.</param>
        /// <returns>A paginated list of students with additional v2 metadata.</returns>
        /// <response code="200">Returns the enhanced list of students.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<object>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStudents(
            [FromQuery] QueryParameters queryParams,
            [FromHeader(Name = "X-Request-Id")] string? xRequestId = null)
        {
            var result = await _studentService.GetAllStudentsAsync(queryParams);
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            // v2 enhancement: wrap with additional metadata
            var v2Response = new
            {
                apiVersion = "2.0",
                generatedAt = DateTime.UtcNow,
                requestId = xRequestId ?? "N/A",
                data = result.Data,
                success = result.Success,
                message = result.Message
            };

            return Ok(v2Response);
        }

        /// <summary>
        /// [v2] Retrieves a specific student by ID with additional computed fields (age, enrollmentCount).
        /// </summary>
        /// <param name="id">The student ID (integer).</param>
        /// <returns>The enhanced student response with computed fields.</returns>
        /// <response code="200">Returns the enhanced student.</response>
        /// <response code="404">Student not found.</response>
        [HttpGet("{id:int}", Name = "GetStudentByIdV2")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            var result = await _studentService.GetStudentByIdAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result);
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            var student = result.Data;
            int age = 0;
            if (student?.DateOfBirth != null)
            {
                var today = DateTime.Today;
                age = today.Year - student.DateOfBirth.Year;
                if (student.DateOfBirth.Date > today.AddYears(-age)) age--;
            }

            // v2 enhancement: add computed fields
            var v2Student = new
            {
                apiVersion = "2.0",
                studentId = student?.StudentId,
                fullName = student?.FullName,
                email = student?.Email,
                dateOfBirth = student?.DateOfBirth,
                // Computed fields exclusive to v2
                age = age,
                enrollmentCount = student?.Enrollments?.Count ?? 0,
                enrollments = student?.Enrollments
            };

            return Ok(ApiResponse<object>.SuccessResponse(v2Student));
        }

        /// <summary>
        /// [v2] Creates a new student. Requires authentication.
        /// </summary>
        /// <param name="request">The student creation request.</param>
        /// <returns>The newly created student.</returns>
        /// <response code="201">Student created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Unauthorized.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// [v2] Deletes a student by ID. Requires Admin role.
        /// </summary>
        /// <param name="id">The student ID to delete.</param>
        /// <returns>Deletion confirmation.</returns>
        /// <response code="200">Student deleted successfully.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden — Admin role required.</response>
        /// <response code="404">Student not found.</response>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
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
