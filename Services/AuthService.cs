using BTBackendOnline2.Models;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using DotNetEnv;
using Autofac.Core;

namespace ISC_ELIB_SERVER.Services
{
    public class AuthService : ILoginService
    {
        private readonly isc_dbContext _context;
        private readonly TokenRequiment jwt;
        private readonly IRefreshToken _refreshTokenService;

        public AuthService(isc_dbContext context, IRefreshToken refreshTokenService)
        {
            _context = context;

            jwt = new()
            {
                SecretKey = Env.GetString("JWT_SECRET_KEY"),
                Issuer = Env.GetString("JWT_ISSUER"),
                Audience = Env.GetString("JWT_AUDIENCE"),
                Subject = Env.GetString("JWT_SUBJECT")
            };
            _refreshTokenService = refreshTokenService;
        }

        public ApiResponse<LoginResponse> AuthLogin(LoginReq request)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == ComputeSha256(request.Password));

                if (user == null)
                {
                    return ApiResponse<LoginResponse>.Fail("Tên đăng nhập hoặc mật khẩu không đúng");
                }

                if (!user.Active)
                {
                    return ApiResponse<LoginResponse>.Fail("Tài khoản đã dừng hoạt động");
                }

                var token = GenerateTokens(user);

                if (string.IsNullOrEmpty(token.Item1))
                {
                    return ApiResponse<LoginResponse>.Fail("Không thể tạo AccessToken");
                }

                if (token.Item2 == null)
                {
                    return ApiResponse<LoginResponse>.Fail("Không thể tạo RefreshToken");
                }

                string roleName = user.Role?.Name ?? "Student";

                var refreshToken = new RefreshTokenRequest()
                {
                    Token = token.Item2.Token,
                    UserId = user.Id,
                    ExpireDate = token.Item2.ExpireDate,
                    Email = token.Item2.Email
                };

                var response = _refreshTokenService.Create(refreshToken);

                if (response.Code == 1)
                {
                    return ApiResponse<LoginResponse>.Fail("Lỗi khi xác thực người dùng");
                }

                return ApiResponse<LoginResponse>.Success(new LoginResponse
                {
                    AccessToken = token.Item1,
                    RefreshToken = response.Data,
                    User = new UserResponseLogin
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        // Avatar = user.Avatar,
                        Role = roleName.ToUpper(),

                    }
                });
            }
            catch (Exception e)
            {
                return ApiResponse<LoginResponse>.Fail("Lỗi khi xác thực người dùng: " + e.Message);
            }

        }
        public ApiResponse<LoginResponse> AuthRefreshToken(string request)
        {
            try
            {
                var refreshToken = _context.RefreshTokens.FirstOrDefault(a => a.Token == request);

                if (refreshToken == null || refreshToken.ExpireDate < DateTime.UtcNow)
                {
                    return ApiResponse<LoginResponse>.Fail("Refresh Token không hợp lệ hoặc đã hết hạn");
                }

                var user = _context.Users.FirstOrDefault(u => u.Id == refreshToken.UserId);

                if (user == null)
                {
                    return ApiResponse<LoginResponse>.Fail("Không tìm thấy refreshtoken của user");
                }



                var token = GenerateTokens(user, refreshToken);

                if (string.IsNullOrEmpty(token.Item1))
                {
                    return ApiResponse<LoginResponse>.Fail("Không thể tạo AccessToken");
                }

                if (token.Item2 == null)
                {
                    return ApiResponse<LoginResponse>.Fail("Không thể tạo RefreshToken");
                }

                var response = _refreshTokenService.Update(refreshToken.Id, refreshToken);

                if (response.Code == 1)
                {
                    return ApiResponse<LoginResponse>.Fail("Lỗi khi xác thực người dùng");
                }

                string roleName = user.Role?.Name ?? "Student";

                return ApiResponse<LoginResponse>.Success(new LoginResponse
                {
                    AccessToken = token.Item1,
                    RefreshToken = response.Data,
                    User = new UserResponseLogin
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        // Avatar = user.Avatar,
                        Role = roleName.ToUpper()
                    }
                });
            }
            catch (Exception e)
            {
                return ApiResponse<LoginResponse>.Fail("Lỗi khi xác thực người dùng: " + e.Message);
            }

        }

        public (string, RefreshToken?) GenerateTokens(User user,
                                                            int accessExpire = 75,
                                                            int refreshExpire = 600)
        {

            var role = _context.Roles.FirstOrDefault(a => a.Id == user.RoleId);
            var roleName = role?.Name ?? "Student";

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("Id", user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenAccess = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(accessExpire),
                signingCredentials: signIn
            );

            var tokenRefresh = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(refreshExpire),
                signingCredentials: signIn
            );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(tokenAccess);

            RefreshToken refreshToken = new()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenRefresh),
                ExpireDate = DateTime.UtcNow.AddMinutes(refreshExpire),
                Email = user.Email
            };

            return (accessToken, refreshToken);
        }

        public (string?, RefreshToken?) GenerateTokens(User user,
                                                            RefreshToken comparedToken,
                                                            int accessExpire = 75,
                                                            int refreshExpire = 600)
        {
            if (comparedToken == null || comparedToken.ExpireDate < DateTime.UtcNow)
            {
                return (null, null);
            }

            var role = _context.Roles.FirstOrDefault(a => a.Id == user.RoleId);

            if (role == null || string.IsNullOrEmpty(role.Name))
            {
                throw new Exception("User role is not valid");
            }

            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                throw new Exception("User is not valid");
            }

            var claims = new[] {
                 new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                 new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToString()),
                 new Claim(ClaimTypes.Name, user.Email),
                 new Claim(ClaimTypes.Role, role.Name),
                 new Claim("Id", user.Id.ToString()),
                 new Claim("Email", user.Email) };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(accessExpire),
                signingCredentials: signIn
             );

            var tokenRefresh = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(refreshExpire),
                signingCredentials: signIn
            );

            string accessTokens = new JwtSecurityTokenHandler().WriteToken(token);

            RefreshToken refreshToken = new()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenRefresh),
                ExpireDate = DateTime.UtcNow.AddMinutes(refreshExpire),
                Email = user.Email
            };

            return (accessTokens, refreshToken);
        }

        public static string ComputeSha256(string input)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + "ledang"));
            StringBuilder builder = new();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
