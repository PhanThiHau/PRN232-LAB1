namespace PRN232.LMS.Services.ResponseModels
{
    public class CourseResponse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public SemesterResponse? Semester { get; set; }
        public List<EnrollmentResponse>? Enrollments { get; set; }
    }
}
