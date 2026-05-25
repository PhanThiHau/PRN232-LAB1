namespace PRN232.LMS.Services.BusinessModels
{
    public class CourseBM
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public SemesterBM? Semester { get; set; }
        public List<EnrollmentBM> Enrollments { get; set; } = new List<EnrollmentBM>();
    }
}
