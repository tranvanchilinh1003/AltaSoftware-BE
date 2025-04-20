using BTBackendOnline2.Models;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class LoginResponse
    {
        public string? AccessToken { get; set; }
        public UserResponseLogin? User { get; set; }
        public RefreshTokenResponse? RefreshToken { get; set; }
    }

    public class UserResponseLogin
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Role { get; set; }
    }
}
