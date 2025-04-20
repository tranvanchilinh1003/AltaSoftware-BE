using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ClassesMapper : Profile
    {
        public ClassesMapper()
        {
            CreateMap<User, ClassUserResponse>();
            CreateMap<ClassSubject, ClassSubjectResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Subject.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Subject.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.HoursSemester1, opt => opt.MapFrom(src => src.HoursSemester1)) 
                .ForMember(dest => dest.HoursSemester2, opt => opt.MapFrom(src => src.HoursSemester2));

            CreateMap<Class, ClassesResponse>();
            CreateMap<ClassesRequest, Class>();
        }
    }
}
