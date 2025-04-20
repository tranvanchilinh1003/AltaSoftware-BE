using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IRefreshToken
    {
        ApiResponse<RefreshTokenResponse> Create(RefreshTokenRequest request);
        ApiResponse<RefreshTokenResponse> Update(int Id , RefreshToken request);
    }
}
