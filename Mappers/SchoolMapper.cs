using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class SchoolMapper : Profile
    {
        public SchoolMapper()
        {
            CreateMap<Campus, CampusResponse>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User != null ? src.User : null));
            CreateMap<User, UserResponseSchool>();
            CreateMap<EducationLevel, EducationLevelResponseSchool>();
            CreateMap<School, SchoolResponse>();
            CreateMap<SchoolRequest, School>();
        }
    }
}
