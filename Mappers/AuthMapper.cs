using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using AutoMapper;

namespace ISC_ELIB_SERVER.Mappers
{
    public class AuthMapper : Profile
    {
        public AuthMapper()
        {
            CreateMap<LoginResponse, AccessTokenReq>();
        }
    }
}
