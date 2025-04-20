using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class QuestionQaMapper : Profile
    {
        public QuestionQaMapper()
        {
            // Mapping từ Entity sang DTO Response
            CreateMap<QuestionQa, QuestionQaResponse>();
            // Mapping từ DTO Request sang Entity
            CreateMap<QuestionQaRequest, QuestionQa>();
        }
    }
}
