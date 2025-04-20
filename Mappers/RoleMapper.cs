using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            // us - res
            CreateMap<Role, RoleResponse>();
            // res - us
            CreateMap<RoleRequest, Role>();
        }
    }
}
