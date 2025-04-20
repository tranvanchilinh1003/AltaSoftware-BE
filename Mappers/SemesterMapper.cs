using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class SemesterMapper : Profile
    {
        public SemesterMapper()
        {
            CreateMap<Semester, SemesterResponse>();
            CreateMap<SemesterRequest, Semester>();
        }
    }
}
