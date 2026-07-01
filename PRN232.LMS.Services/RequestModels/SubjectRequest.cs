using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.RequestModels
{
    /// <summary>Request model for creating a new subject.</summary>
    public class CreateSubjectRequest
    {
        /// <summary>Subject code (e.g. PRN231, DBI202).</summary>
        [Required(ErrorMessage = "SubjectCode is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "SubjectCode must be between 4 and 20 characters.")]
        [RegularExpression(@"^[A-Z]{2,3}\d{3}$", ErrorMessage = "SubjectCode must follow the format: 2-3 uppercase letters followed by 3 digits (e.g. PRN231, DBI202).")]
        public string SubjectCode { get; set; } = string.Empty;

        /// <summary>Full name of the subject.</summary>
        [Required(ErrorMessage = "SubjectName is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "SubjectName must be between 3 and 100 characters.")]
        public string SubjectName { get; set; } = string.Empty;

        /// <summary>Number of credits (1–10).</summary>
        [Required(ErrorMessage = "Credit is required.")]
        [Range(1, 10, ErrorMessage = "Credit must be between 1 and 10.")]
        public int Credit { get; set; }
    }

    /// <summary>Request model for updating an existing subject.</summary>
    public class UpdateSubjectRequest
    {
        /// <summary>Subject code (e.g. PRN231, DBI202).</summary>
        [Required(ErrorMessage = "SubjectCode is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "SubjectCode must be between 4 and 20 characters.")]
        [RegularExpression(@"^[A-Z]{2,3}\d{3}$", ErrorMessage = "SubjectCode must follow the format: 2-3 uppercase letters followed by 3 digits (e.g. PRN231, DBI202).")]
        public string SubjectCode { get; set; } = string.Empty;

        /// <summary>Full name of the subject.</summary>
        [Required(ErrorMessage = "SubjectName is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "SubjectName must be between 3 and 100 characters.")]
        public string SubjectName { get; set; } = string.Empty;

        /// <summary>Number of credits (1–10).</summary>
        [Required(ErrorMessage = "Credit is required.")]
        [Range(1, 10, ErrorMessage = "Credit must be between 1 and 10.")]
        public int Credit { get; set; }
    }
}
