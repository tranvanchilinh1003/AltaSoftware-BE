using AutoMapper;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Mappers
{
    public class QuestionViewMapper : Profile
    {
        public QuestionViewMapper()
        {
            CreateMap<QuestionView, QuestionViewResponse>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.ViewCount, opt => opt.Ignore()); // ViewCount tính riêng
        }
    }
}
