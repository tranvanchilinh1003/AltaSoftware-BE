using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TemporaryLeaveMapper : Profile
    {
        public TemporaryLeaveMapper()
        {
            CreateMap<TemporaryLeave, TemporaryLeave_AddRequest>().ReverseMap();
            CreateMap<TemporaryLeave, TemporaryLeave_UpdateRequest>().ReverseMap().ForMember(des => des.Id, opt => opt.Ignore());
            CreateMap<TemporaryLeaveResponse, TemporaryLeave>().ReverseMap();

        }
    }
}
