using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class EducationLevelMapper : Profile
    {
        public EducationLevelMapper()
        {
            CreateMap<EducationLevel, EducationLevelResponse>();
            CreateMap<EducationLevelRequest, EducationLevel>();
        }
    }
}
