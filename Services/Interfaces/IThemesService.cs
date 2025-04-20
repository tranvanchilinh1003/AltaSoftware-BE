using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IThemesService
    {
            ApiResponse<ICollection<ThemesResponse>> GetThemes(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
            ApiResponse<ThemesResponse> GetThemesById(long id);
            ApiResponse<ThemesResponse> CreateThemes(ThemesRequest themesRequest);
            ApiResponse<ThemesResponse> UpdateThemes(long id, ThemesRequest themesRequest);
            ApiResponse<Theme> DeleteThemes(long id);
    }
}
