namespace PRN232.LMS.Services.BusinessModels
{
    public class SemesterBM
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CourseBM> Courses { get; set; } = new List<CourseBM>();
    }
}
