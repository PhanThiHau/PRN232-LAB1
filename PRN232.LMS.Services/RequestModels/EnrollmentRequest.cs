using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.RequestModels
{
    public class CreateEnrollmentRequest
    {
        [Required(ErrorMessage = "StudentId is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "CourseId is required.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "EnrollDate is required.")]
        public DateTime EnrollDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [MaxLength(20, ErrorMessage = "Status must not exceed 20 characters.")]
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateEnrollmentRequest
    {
        [Required(ErrorMessage = "StudentId is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "CourseId is required.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "EnrollDate is required.")]
        public DateTime EnrollDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [MaxLength(20, ErrorMessage = "Status must not exceed 20 characters.")]
        public string Status { get; set; } = string.Empty;
    }
}
