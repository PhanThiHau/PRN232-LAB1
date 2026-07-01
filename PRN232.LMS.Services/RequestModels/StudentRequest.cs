using System.ComponentModel.DataAnnotations;
using PRN232.LMS.Services.Validation;

namespace PRN232.LMS.Services.RequestModels
{
    /// <summary>Query parameters for listing/searching students.</summary>
    public class StudentQueryRequest
    {
        /// <summary>Search by full name (partial match).</summary>
        public string? Search { get; set; }

        /// <summary>Field to sort by.</summary>
        public string? SortBy { get; set; }

        /// <summary>Sort direction: asc or desc.</summary>
        public string? SortOrder { get; set; }

        /// <summary>Page number (1-based).</summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page must be at least 1.")]
        public int Page { get; set; } = 1;

        /// <summary>Number of items per page.</summary>
        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;

        /// <summary>Comma-separated list of fields to include in the response.</summary>
        public string? Fields { get; set; }

        /// <summary>Comma-separated list of related resources to expand (e.g., enrollments).</summary>
        public string? Expand { get; set; }
    }

    /// <summary>Request model for creating a new student.</summary>
    public class CreateStudentRequest
    {
        /// <summary>Student's full name.</summary>
        [Required(ErrorMessage = "FullName is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "FullName must be between 2 and 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>Student's email address.</summary>
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Student's date of birth.</summary>
        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>FPTU student code (e.g. SE19886, CE18793).</summary>
        [FptuStudentCode]
        public string? StudentCode { get; set; }

        /// <summary>Student's phone number (optional).</summary>
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone must not exceed 15 characters.")]
        public string? Phone { get; set; }
    }

    /// <summary>Request model for updating an existing student.</summary>
    public class UpdateStudentRequest
    {
        /// <summary>Student's full name.</summary>
        [Required(ErrorMessage = "FullName is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "FullName must be between 2 and 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>Student's email address.</summary>
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Student's date of birth.</summary>
        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>FPTU student code (e.g. SE19886, CE18793).</summary>
        [FptuStudentCode]
        public string? StudentCode { get; set; }

        /// <summary>Student's phone number (optional).</summary>
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone must not exceed 15 characters.")]
        public string? Phone { get; set; }
    }
}
