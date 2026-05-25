using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.RequestModels
{
    public class CreateSubjectRequest
    {
        [Required(ErrorMessage = "SubjectCode is required.")]
        [MaxLength(20, ErrorMessage = "SubjectCode must not exceed 20 characters.")]
        public string SubjectCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "SubjectName is required.")]
        [MaxLength(100, ErrorMessage = "SubjectName must not exceed 100 characters.")]
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Credit is required.")]
        [Range(1, 10, ErrorMessage = "Credit must be between 1 and 10.")]
        public int Credit { get; set; }
    }

    public class UpdateSubjectRequest
    {
        [Required(ErrorMessage = "SubjectCode is required.")]
        [MaxLength(20, ErrorMessage = "SubjectCode must not exceed 20 characters.")]
        public string SubjectCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "SubjectName is required.")]
        [MaxLength(100, ErrorMessage = "SubjectName must not exceed 100 characters.")]
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Credit is required.")]
        [Range(1, 10, ErrorMessage = "Credit must be between 1 and 10.")]
        public int Credit { get; set; }
    }
}
