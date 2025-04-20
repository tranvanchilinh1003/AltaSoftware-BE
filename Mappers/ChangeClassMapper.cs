using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ChangeClassMapper : Profile
    {
        public ChangeClassMapper()
        {
            CreateMap<ChangeClass, ChangeClass_AddRequest>().ReverseMap();
            CreateMap<ChangeClass, ChangeClass_UpdateRequest>().ReverseMap();

            CreateMap<ChangeClassResponse, ChangeClass>().ReverseMap();
        }
    }
}
