using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ILoginService
    {
        ApiResponse<LoginResponse> AuthLogin(LoginReq request);
        ApiResponse<LoginResponse> AuthRefreshToken(string token);
    }
}
