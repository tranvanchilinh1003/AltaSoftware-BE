using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Models.Responses;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ExamMapper : Profile
    {
        public ExamMapper()
        {
            // ex - resp
            CreateMap<Exam, ExamResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));


            // req - ex
            CreateMap<ExamRequest, Exam>();
        }
    }
}
