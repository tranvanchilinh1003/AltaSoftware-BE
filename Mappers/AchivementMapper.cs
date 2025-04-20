using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class AchivementMapper : Profile
    {
        public AchivementMapper()
        {
            // us - res
            CreateMap<Achievement, AchivementResponse>();
            // res - us
            CreateMap<AchivementRequest, Achievement>();
        }
    }
}
