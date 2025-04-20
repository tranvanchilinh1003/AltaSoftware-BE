using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class RolePermissionMapper : Profile
    {
        public RolePermissionMapper() 
        {
            // us - res
            CreateMap<RolePermission, RolePermissionResponse>();
            // res - us
            CreateMap<RolePermissionRequest, RolePermission>();
        }
    }
}
