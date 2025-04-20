using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using System.Linq;
namespace ISC_ELIB_SERVER.Mappers
{
    public class TeacherListMapper : Profile
    {
        public TeacherListMapper()
        {
            CreateMap<TeacherInfo, TeacherListResponse>()
                // Map Id
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User != null ? src.User.Id : 0)) 
                .ForMember(dest => dest.TeacherCode, opt => opt.MapFrom(src => src.User != null ? src.User.Code : ""))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : ""))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.User != null && src.User.Dob.HasValue
                                                                           ? src.User.Dob.Value
                                                                           : default(DateTime)))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src =>
                    src.User != null ? src.User.Gender : false))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.Retirements != null && src.Retirements.Any()
                        ? src.Retirements.OrderByDescending(r => r.Date).FirstOrDefault().Status
                        : RetirementStatus.Working));
        }
    }
}
