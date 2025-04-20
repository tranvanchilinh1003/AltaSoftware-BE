using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ExamGraderMapper : Profile
    {
        public ExamGraderMapper()
        {
            CreateMap<ExamGrader, ExamGraderResponse>()
                  .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null)); ;
            CreateMap<ExamGraderRequest, ExamGrader>();
        }
    }
}
