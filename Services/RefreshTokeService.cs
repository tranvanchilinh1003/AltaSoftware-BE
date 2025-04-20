using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class RefreshTokeService : IRefreshToken
    {
        private readonly RefreshTokenRepo _repository;
        public RefreshTokeService(RefreshTokenRepo repository)
        {
            _repository = repository;
        }
        public ApiResponse<RefreshTokenResponse> Create(RefreshTokenRequest request)
        {
            try
            {
                request.ExpireDate = DateTime.SpecifyKind(request.ExpireDate, DateTimeKind.Unspecified);

                var refreshToken = new RefreshToken
                {
                    UserId = request.UserId,
                    ExpireDate = request.ExpireDate,
                    Email = request.Email,
                    Token = request.Token,
                };

                var created = _repository.Create(refreshToken);

                var response = new RefreshTokenResponse
                {
                    UserId = request.UserId,
                    ExpireDate = created.ExpireDate,
                    Email = created.Email,
                    Token = created.Token,
                };

                return ApiResponse<RefreshTokenResponse>.Success(response);
            }
            catch
            {
                return ApiResponse<RefreshTokenResponse>.BadRequest("Lỗi khi tạo refresh token");
            }
        }

        public ApiResponse<RefreshTokenResponse> Update(int id ,RefreshToken request)
        {
            try
            {

                var existing = _repository.GetById(id);

                existing.Token = request.Token;
                existing.ExpireDate = DateTime.SpecifyKind(request.ExpireDate, DateTimeKind.Unspecified);

                var update = _repository.Update(existing);

                var response = new RefreshTokenResponse
                {
                    ExpireDate = update.ExpireDate,
                    Email = update.Email,
                    Token = update.Token,
                    UserId = update.UserId,
                };

                return ApiResponse<RefreshTokenResponse>.Success(response);
            }
            catch
            {
                return ApiResponse<RefreshTokenResponse>.BadRequest("Lỗi khi tạo refresh token");
            }
        }

    }
}
