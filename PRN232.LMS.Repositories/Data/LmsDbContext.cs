using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Data
{
    public class LmsDbContext : DbContext
    {
        public LmsDbContext(DbContextOptions<LmsDbContext> options) : base(options)
        {
        }

        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEntities(modelBuilder);
            SeedSemesters(modelBuilder);
            SeedSubjects(modelBuilder);
            SeedCourses(modelBuilder);
            SeedStudents(modelBuilder);
            SeedEnrollments(modelBuilder);
            SeedUsers(modelBuilder);
        }

        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Semester>(entity =>
            {
                entity.HasKey(e => e.SemesterId);
                entity.Property(e => e.SemesterName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.SubjectId);
                entity.Property(e => e.SubjectCode).IsRequired().HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.SubjectName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Credit).IsRequired();
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId);
                entity.Property(e => e.CourseName).IsRequired().HasMaxLength(100);
                entity.HasOne(e => e.Semester)
                      .WithMany(s => s.Courses)
                      .HasForeignKey(e => e.SemesterId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.DateOfBirth).IsRequired();
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.EnrollmentId);
                entity.Property(e => e.EnrollDate).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).IsUnicode(false);
                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrollments)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50).IsUnicode(false);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20).IsUnicode(false);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired().HasMaxLength(512).IsUnicode(false);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void SeedUsers(ModelBuilder modelBuilder)
        {
            // Passwords are BCrypt hashed:
            // admin -> 123456  (BCrypt hash verified)
            // user  -> 123456  (BCrypt hash verified)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = "$2a$11$KfhLqh6JLIldisT5u4ExOOu5umy9Xxeqmr5hBWkj84dt8nyKjUg5a",
                    Role = "Admin"
                },
                new User
                {
                    UserId = 2,
                    Username = "user",
                    PasswordHash = "$2a$11$KfhLqh6JLIldisT5u4ExOOu5umy9Xxeqmr5hBWkj84dt8nyKjUg5a",
                    Role = "User"
                }
            );
        }

        private void SeedSemesters(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Semester>().HasData(
                new Semester { SemesterId = 1, SemesterName = "Fall 2024", StartDate = new DateTime(2024, 9, 1), EndDate = new DateTime(2024, 12, 31) },
                new Semester { SemesterId = 2, SemesterName = "Spring 2025", StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 5, 15) },
                new Semester { SemesterId = 3, SemesterName = "Summer 2025", StartDate = new DateTime(2025, 6, 1), EndDate = new DateTime(2025, 8, 31) },
                new Semester { SemesterId = 4, SemesterName = "Fall 2025", StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2025, 12, 31) },
                new Semester { SemesterId = 5, SemesterName = "Spring 2026", StartDate = new DateTime(2026, 1, 15), EndDate = new DateTime(2026, 5, 15) }
            );
        }

        private void SeedSubjects(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectId = 1, SubjectCode = "PRN231", SubjectName = "Building Cross-Platform Back-End Application With .NET", Credit = 3 },
                new Subject { SubjectId = 2, SubjectCode = "PRN232", SubjectName = "Building Cross-Platform Back-End Application With .NET (Advanced)", Credit = 3 },
                new Subject { SubjectId = 3, SubjectCode = "DBI202", SubjectName = "Database Systems", Credit = 3 },
                new Subject { SubjectId = 4, SubjectCode = "SWR302", SubjectName = "Software Requirement", Credit = 3 },
                new Subject { SubjectId = 5, SubjectCode = "SWD392", SubjectName = "Software Architecture and Design", Credit = 3 },
                new Subject { SubjectId = 6, SubjectCode = "MAS291", SubjectName = "Statistics and Probability", Credit = 3 },
                new Subject { SubjectId = 7, SubjectCode = "IOT102", SubjectName = "Internet of Things", Credit = 3 },
                new Subject { SubjectId = 8, SubjectCode = "WEB301", SubjectName = "Web Development with JavaScript", Credit = 3 },
                new Subject { SubjectId = 9, SubjectCode = "MLN111", SubjectName = "Philosophy of Marxism-Leninism", Credit = 3 },
                new Subject { SubjectId = 10, SubjectCode = "SEP490", SubjectName = "SE Capstone Project", Credit = 5 }
            );
        }

        private void SeedCourses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, CourseName = "PRN231 - Fall 2024 - SE1801", SemesterId = 1 },
                new Course { CourseId = 2, CourseName = "PRN232 - Fall 2024 - SE1802", SemesterId = 1 },
                new Course { CourseId = 3, CourseName = "DBI202 - Fall 2024 - SE1803", SemesterId = 1 },
                new Course { CourseId = 4, CourseName = "SWR302 - Fall 2024 - SE1804", SemesterId = 1 },
                new Course { CourseId = 5, CourseName = "PRN231 - Spring 2025 - SE1805", SemesterId = 2 },
                new Course { CourseId = 6, CourseName = "PRN232 - Spring 2025 - SE1806", SemesterId = 2 },
                new Course { CourseId = 7, CourseName = "SWD392 - Spring 2025 - SE1807", SemesterId = 2 },
                new Course { CourseId = 8, CourseName = "MAS291 - Spring 2025 - SE1808", SemesterId = 2 },
                new Course { CourseId = 9, CourseName = "IOT102 - Summer 2025 - SE1809", SemesterId = 3 },
                new Course { CourseId = 10, CourseName = "WEB301 - Summer 2025 - SE1810", SemesterId = 3 },
                new Course { CourseId = 11, CourseName = "MLN111 - Summer 2025 - SE1811", SemesterId = 3 },
                new Course { CourseId = 12, CourseName = "SEP490 - Summer 2025 - SE1812", SemesterId = 3 },
                new Course { CourseId = 13, CourseName = "PRN231 - Fall 2025 - SE1813", SemesterId = 4 },
                new Course { CourseId = 14, CourseName = "DBI202 - Fall 2025 - SE1814", SemesterId = 4 },
                new Course { CourseId = 15, CourseName = "SWR302 - Fall 2025 - SE1815", SemesterId = 4 },
                new Course { CourseId = 16, CourseName = "SWD392 - Fall 2025 - SE1816", SemesterId = 4 },
                new Course { CourseId = 17, CourseName = "PRN232 - Spring 2026 - SE1817", SemesterId = 5 },
                new Course { CourseId = 18, CourseName = "MAS291 - Spring 2026 - SE1818", SemesterId = 5 },
                new Course { CourseId = 19, CourseName = "WEB301 - Spring 2026 - SE1819", SemesterId = 5 },
                new Course { CourseId = 20, CourseName = "SEP490 - Spring 2026 - SE1820", SemesterId = 5 }
            );
        }

        private void SeedStudents(ModelBuilder modelBuilder)
        {
            var students = new List<Student>();
            string[] firstNames = { "Nguyen Van", "Tran Thi", "Le Hoang", "Pham Minh", "Hoang Duc", "Vo Thanh", "Dang Quoc", "Bui Thi", "Do Xuan", "Ngo Hai" };
            string[] lastNames = { "An", "Binh", "Cuong", "Dung", "Em", "Phuong", "Giang", "Hoa", "Khanh", "Linh" };

            int studentId = 1;
            for (int i = 0; i < firstNames.Length; i++)
            {
                for (int j = 0; j < lastNames.Length; j++)
                {
                    if (studentId > 50) break;
                    students.Add(new Student
                    {
                        StudentId = studentId,
                        FullName = $"{firstNames[i]} {lastNames[j]}",
                        Email = $"student{studentId:D3}@fpt.edu.vn",
                        DateOfBirth = new DateTime(2000 + (studentId % 5), (studentId % 12) + 1, (studentId % 28) + 1)
                    });
                    studentId++;
                }
            }

            modelBuilder.Entity<Student>().HasData(students);
        }

        private void SeedEnrollments(ModelBuilder modelBuilder)
        {
            var enrollments = new List<Enrollment>();
            string[] statuses = { "Active", "Completed", "Dropped", "Pending", "Withdrawn" };

            int enrollmentId = 1;
            for (int studentId = 1; studentId <= 50; studentId++)
            {
                int numberOfEnrollments = 10;
                for (int e = 0; e < numberOfEnrollments; e++)
                {
                    int courseId = ((studentId + e) % 20) + 1;
                    string status = statuses[(studentId + e) % statuses.Length];
                    int month = ((studentId + e) % 12) + 1;
                    int day = ((studentId + e) % 28) + 1;
                    int year = 2024 + ((studentId + e) % 3);

                    enrollments.Add(new Enrollment
                    {
                        EnrollmentId = enrollmentId,
                        StudentId = studentId,
                        CourseId = courseId,
                        EnrollDate = new DateTime(year, month, day),
                        Status = status
                    });
                    enrollmentId++;
                }
            }

            modelBuilder.Entity<Enrollment>().HasData(enrollments);
        }
    }
}
