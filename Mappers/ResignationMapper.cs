using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ResignationMapper : Profile
    {
        public ResignationMapper() 
        {
            CreateMap<Resignation, ResignationResponse>();
            CreateMap<ResignationRequest, Resignation>();
        }
    }
}
