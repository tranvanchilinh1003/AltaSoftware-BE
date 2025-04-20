using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.Mappers
{
    public class SupportMapper : Profile
    {
        public SupportMapper()
        {
            CreateMap<Support, SupportResponse>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => new SupportTypeResponse
                {
                    Id = (int)src.Type,
                    Name = Enum.GetName(typeof(SupportType), src.Type) 
                }))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User != null ? new UserSuppostResponse
                {
                    Id = src.User.Id,
                    Code = src.User.Code,
                    FullName = src.User.FullName
                } : null));

            CreateMap<SupportRequest, Support>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (SupportType)src.Type));

            CreateMap<User, UserSuppostResponse>();

            CreateMap<SupportType, SupportTypeResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int)src))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => Enum.GetName(typeof(SupportType), src)));
        }
    }
}
