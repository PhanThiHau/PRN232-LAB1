using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.RequestModels
{
    /// <summary>Request model for creating a new enrollment.</summary>
    public class CreateEnrollmentRequest
    {
        /// <summary>ID of the student to enroll.</summary>
        [Required(ErrorMessage = "StudentId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "StudentId must be a positive integer.")]
        public int StudentId { get; set; }

        /// <summary>ID of the course to enroll in.</summary>
        [Required(ErrorMessage = "CourseId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be a positive integer.")]
        public int CourseId { get; set; }

        /// <summary>Date of enrollment.</summary>
        [Required(ErrorMessage = "EnrollDate is required.")]
        public DateTime EnrollDate { get; set; }

        /// <summary>Enrollment status: Active, Completed, Dropped, Pending, or Withdrawn.</summary>
        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status must not exceed 20 characters.")]
        [RegularExpression(@"^(Active|Completed|Dropped|Pending|Withdrawn)$",
            ErrorMessage = "Status must be one of: Active, Completed, Dropped, Pending, Withdrawn.")]
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>Request model for updating an existing enrollment.</summary>
    public class UpdateEnrollmentRequest
    {
        /// <summary>ID of the student.</summary>
        [Required(ErrorMessage = "StudentId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "StudentId must be a positive integer.")]
        public int StudentId { get; set; }

        /// <summary>ID of the course.</summary>
        [Required(ErrorMessage = "CourseId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be a positive integer.")]
        public int CourseId { get; set; }

        /// <summary>Date of enrollment.</summary>
        [Required(ErrorMessage = "EnrollDate is required.")]
        public DateTime EnrollDate { get; set; }

        /// <summary>Enrollment status: Active, Completed, Dropped, Pending, or Withdrawn.</summary>
        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status must not exceed 20 characters.")]
        [RegularExpression(@"^(Active|Completed|Dropped|Pending|Withdrawn)$",
            ErrorMessage = "Status must be one of: Active, Completed, Dropped, Pending, Withdrawn.")]
        public string Status { get; set; } = string.Empty;
    }
}
