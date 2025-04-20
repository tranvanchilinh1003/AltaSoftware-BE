using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses.ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ScoreTypeMapper : Profile
    {
        public ScoreTypeMapper()
        {
            CreateMap<ScoreType, ScoreTypeResponse>();

            CreateMap<ScoreTypeRequest, ScoreType>();

        }
    }
}
