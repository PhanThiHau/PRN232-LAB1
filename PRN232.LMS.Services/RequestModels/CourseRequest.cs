using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.RequestModels
{
    /// <summary>Request model for creating a new course.</summary>
    public class CreateCourseRequest
    {
        /// <summary>Name of the course.</summary>
        [Required(ErrorMessage = "CourseName is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "CourseName must be between 3 and 100 characters.")]
        public string CourseName { get; set; } = string.Empty;

        /// <summary>ID of the semester this course belongs to.</summary>
        [Required(ErrorMessage = "SemesterId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "SemesterId must be a positive integer.")]
        public int SemesterId { get; set; }
    }

    /// <summary>Request model for updating an existing course.</summary>
    public class UpdateCourseRequest
    {
        /// <summary>Name of the course.</summary>
        [Required(ErrorMessage = "CourseName is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "CourseName must be between 3 and 100 characters.")]
        public string CourseName { get; set; } = string.Empty;

        /// <summary>ID of the semester this course belongs to.</summary>
        [Required(ErrorMessage = "SemesterId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "SemesterId must be a positive integer.")]
        public int SemesterId { get; set; }
    }
}
