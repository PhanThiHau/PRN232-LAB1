using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.RequestModels
{
    /// <summary>Request model for creating a new semester.</summary>
    public class CreateSemesterRequest
    {
        /// <summary>Name of the semester (e.g. Summer2024, Fall2024).</summary>
        [Required(ErrorMessage = "SemesterName is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "SemesterName must be between 3 and 100 characters.")]
        public string SemesterName { get; set; } = string.Empty;

        /// <summary>Start date of the semester.</summary>
        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        /// <summary>End date of the semester.</summary>
        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
    }

    /// <summary>Request model for updating an existing semester.</summary>
    public class UpdateSemesterRequest
    {
        /// <summary>Name of the semester.</summary>
        [Required(ErrorMessage = "SemesterName is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "SemesterName must be between 3 and 100 characters.")]
        public string SemesterName { get; set; } = string.Empty;

        /// <summary>Start date of the semester.</summary>
        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        /// <summary>End date of the semester.</summary>
        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
    }
}
