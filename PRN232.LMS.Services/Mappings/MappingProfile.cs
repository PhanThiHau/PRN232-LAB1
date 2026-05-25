using AutoMapper;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;

namespace PRN232.LMS.Services.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateEntityToBusinessMaps();
            CreateBusinessToResponseMaps();
            CreateRequestToEntityMaps();
        }

        private void CreateEntityToBusinessMaps()
        {
            CreateMap<Student, StudentBM>().ReverseMap();
            CreateMap<Enrollment, EnrollmentBM>().ReverseMap();
            CreateMap<Course, CourseBM>().ReverseMap();
            CreateMap<Subject, SubjectBM>().ReverseMap();
            CreateMap<Semester, SemesterBM>().ReverseMap();
        }

        private void CreateBusinessToResponseMaps()
        {
            CreateMap<StudentBM, StudentResponse>().ReverseMap();
            CreateMap<EnrollmentBM, EnrollmentResponse>().ReverseMap();
            CreateMap<CourseBM, CourseResponse>().ReverseMap();
            CreateMap<SubjectBM, SubjectResponse>().ReverseMap();
            CreateMap<SemesterBM, SemesterResponse>().ReverseMap();
        }

        private void CreateRequestToEntityMaps()
        {
            CreateMap<CreateStudentRequest, Student>();
            CreateMap<UpdateStudentRequest, Student>();

            CreateMap<CreateEnrollmentRequest, Enrollment>();
            CreateMap<UpdateEnrollmentRequest, Enrollment>();

            CreateMap<CreateCourseRequest, Course>();
            CreateMap<UpdateCourseRequest, Course>();

            CreateMap<CreateSubjectRequest, Subject>();
            CreateMap<UpdateSubjectRequest, Subject>();

            CreateMap<CreateSemesterRequest, Semester>();
            CreateMap<UpdateSemesterRequest, Semester>();

            CreateMap<Student, StudentResponse>();
            CreateMap<Enrollment, EnrollmentResponse>();
            CreateMap<Course, CourseResponse>();
            CreateMap<Subject, SubjectResponse>();
            CreateMap<Semester, SemesterResponse>();
        }
    }
}
