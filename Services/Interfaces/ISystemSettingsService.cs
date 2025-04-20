using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ISystemSettingsService
    {
        ApiResponse<SystemSettingResponse> GetSystemSettingByUser();
        ApiResponse<SystemSettingResponse> CreateOrUpdateSystemSetting(SystemSettingRequest systemSettingRequest);
        ApiResponse<object> DeleteSystemSetting(int id);
    }
}
