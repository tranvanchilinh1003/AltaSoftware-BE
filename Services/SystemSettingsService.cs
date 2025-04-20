using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ISC_ELIB_SERVER.DTOs.Responses.ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly IThemesService _themesService;
        private readonly ISystemSettingsRepo _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SystemSettingsService(ISystemSettingsRepo repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IThemesService themesService)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _themesService = themesService;
        }

        private int? GetUserIdFromToken()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : (int?)null;
        }


        public ApiResponse<SystemSettingResponse> GetSystemSettingByUser()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return ApiResponse<SystemSettingResponse>.Unauthorized("Người dùng chưa đăng nhập.");

            var setting = _repository.GetAll().FirstOrDefault(ss => ss.UserId == userId);

            if (setting == null)
                return ApiResponse<SystemSettingResponse>.NotFound("Không tìm thấy cài đặt hệ thống của người dùng.");

            var response = _mapper.Map<SystemSettingResponse>(setting);
            return ApiResponse<SystemSettingResponse>.Success(response);
        }

       public ApiResponse<SystemSettingResponse> CreateOrUpdateSystemSetting(SystemSettingRequest systemSettingRequest)
{
    var userId = GetUserIdFromToken();
    if (userId == null)
        return ApiResponse<SystemSettingResponse>.Unauthorized("Người dùng chưa đăng nhập.");

    try
    {
        var themeId = systemSettingRequest.ThemeId;
        var existingTheme = _themesService.GetThemesById(themeId);

        var existingSetting = _repository.GetAll().FirstOrDefault(ss => ss.UserId == userId);

        if (existingSetting != null)
        {
            existingSetting.ThemeId = themeId;
            var updated = _repository.Update(existingSetting);
            return updated == null
                ? ApiResponse<SystemSettingResponse>.Fail("Cập nhật thất bại.")
                : ApiResponse<SystemSettingResponse>.Success(_mapper.Map<SystemSettingResponse>(updated));
        }
        else
        {
            var newSetting = new SystemSetting
            {
                UserId = userId.Value,
                ThemeId = themeId,
                Captcha = true,
                Active = true
            };

            var created = _repository.Create(newSetting);
            return created == null
                ? ApiResponse<SystemSettingResponse>.Fail("Tạo mới thất bại.")
                : ApiResponse<SystemSettingResponse>.Success(_mapper.Map<SystemSettingResponse>(created));
        }
    }
    catch (Exception ex)
    {
           
     return ApiResponse<SystemSettingResponse>.NotFound($"Không tìm thấy");
                
    }
}



        public ApiResponse<object> DeleteSystemSetting(int id)
        {
            var success = _repository.Delete(id);

            if (!success)
                return ApiResponse<object>.NotFound("Không tìm thấy cài đặt hệ thống để xóa.");

            return ApiResponse<object>.Success("Xóa thành công.");
        }
    }
}
