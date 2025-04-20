using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class AnswerImagesQaMapper : Profile
    {
        public AnswerImagesQaMapper()
        {
            CreateMap<AnswerImagesQa, AnswerImagesQaResponse>();
            CreateMap<AnswerImagesQaRequest, AnswerImagesQa>();
        }
    }
}
