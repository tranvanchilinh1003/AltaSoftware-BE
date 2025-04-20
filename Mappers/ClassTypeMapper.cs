using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ClassTypeMapper : Profile
    {
        public ClassTypeMapper()
        {
            CreateMap<ClassType, ClassTypeResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) 
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) 
                .ForMember(dest => dest.AcademicYear, opt => opt.MapFrom(src => src.AcademicYear))
                .ForMember(dest => dest.IsDelete, opt => opt.MapFrom(src => !src.Active));

            CreateMap<ClassTypeRequest, ClassType>()
                .ForMember(dest => dest.AcademicYear, opt => opt.Ignore()); 
        }
    }
}
