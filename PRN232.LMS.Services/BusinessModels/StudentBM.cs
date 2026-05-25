namespace PRN232.LMS.Services.BusinessModels
{
    public class StudentBM
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public List<EnrollmentBM> Enrollments { get; set; } = new List<EnrollmentBM>();
    }
}
