using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TestMapper : Profile
    {
        public TestMapper() {
            CreateMap<Test, TestResponse>()
                .AfterMap((src, dest, context) =>
                {
                    dest.Teacher = context.Mapper.Map<UserResponse>(src.User);
                    dest.Subject = context.Mapper.Map<SubjectResponse>(src.Subject);
                    dest.GradeLevel = context.Mapper.Map<GradeLevelResponse>(src.GradeLevel);
                });
            CreateMap<TestRequest, Test>();
            CreateMap<TestByStudentResponse, TestByStudentResponse>();
        }
    }
}
