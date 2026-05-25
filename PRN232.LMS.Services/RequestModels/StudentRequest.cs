using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.RequestModels
{
    public class CreateStudentRequest
    {
        [Required(ErrorMessage = "FullName is required.")]
        [MaxLength(100, ErrorMessage = "FullName must not exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateTime DateOfBirth { get; set; }
    }

    public class UpdateStudentRequest
    {
        [Required(ErrorMessage = "FullName is required.")]
        [MaxLength(100, ErrorMessage = "FullName must not exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateTime DateOfBirth { get; set; }
    }
}
