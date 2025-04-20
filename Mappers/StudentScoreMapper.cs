using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class StudentScoreMapper : Profile
    {
        public StudentScoreMapper()
        {

            CreateMap<StudentScore, StudentScoreResponse>();
            CreateMap<StudentScoreRequest, StudentScore>();
            CreateMap<Subject, SubjectScoreResponse>();
            CreateMap<Subject, SubjectScoreResponse>();
            CreateMap<Class, ClassScoreResponse>();
            CreateMap<User, StudentResponse>();
            CreateMap<Semester, SemesterScoreResponse>();
            CreateMap<StudentScore, ScoreResponse>();
            CreateMap<ScoreType, TypeScoreResponse>();
        }
    }
}
