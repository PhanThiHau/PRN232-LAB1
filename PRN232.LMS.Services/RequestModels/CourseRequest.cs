using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.RequestModels
{
    public class CreateCourseRequest
    {
        [Required(ErrorMessage = "CourseName is required.")]
        [MaxLength(100, ErrorMessage = "CourseName must not exceed 100 characters.")]
        public string CourseName { get; set; } = string.Empty;

        [Required(ErrorMessage = "SemesterId is required.")]
        public int SemesterId { get; set; }
    }

    public class UpdateCourseRequest
    {
        [Required(ErrorMessage = "CourseName is required.")]
        [MaxLength(100, ErrorMessage = "CourseName must not exceed 100 characters.")]
        public string CourseName { get; set; } = string.Empty;

        [Required(ErrorMessage = "SemesterId is required.")]
        public int SemesterId { get; set; }
    }
}
