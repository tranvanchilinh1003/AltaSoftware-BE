using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ExemptionMapper : Profile
    {
        public ExemptionMapper()
        {
            CreateMap<Exemption, Exemption_AddRequest>().ReverseMap();
            CreateMap<Exemption, Exemption_UpdateRequest>().ReverseMap();

            CreateMap<ExemptionResponse, Exemption>().ReverseMap();
        }
    }
}
