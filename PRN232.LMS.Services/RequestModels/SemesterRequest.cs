using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.RequestModels
{
    public class CreateSemesterRequest
    {
        [Required(ErrorMessage = "SemesterName is required.")]
        [MaxLength(100, ErrorMessage = "SemesterName must not exceed 100 characters.")]
        public string SemesterName { get; set; } = string.Empty;

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
    }

    public class UpdateSemesterRequest
    {
        [Required(ErrorMessage = "SemesterName is required.")]
        [MaxLength(100, ErrorMessage = "SemesterName must not exceed 100 characters.")]
        public string SemesterName { get; set; } = string.Empty;

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
    }
}
