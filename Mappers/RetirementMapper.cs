using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using Microsoft.AspNetCore.Authentication;

namespace ISC_ELIB_SERVER.Mappers
{
    public class RetirementMapper : Profile
    {
        public RetirementMapper() 
        { 
            CreateMap<Retirement, RetirementResponse>();
            CreateMap<RetirementRequest, Retirement>();
        }
    }
}
