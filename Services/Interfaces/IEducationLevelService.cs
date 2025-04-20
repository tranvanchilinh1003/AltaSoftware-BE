using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IEducationLevelService
    {
            ApiResponse<ICollection<EducationLevelResponse>> GetEducationLevels(int? page, int? pageSize, string? sortColumn, string? sortOrder);
            ApiResponse<EducationLevelResponse> GetEducationLevelById(long id);
            ApiResponse<EducationLevelResponse> CreateEducationLevel(EducationLevelRequest EducationLevelRequest);
            ApiResponse<EducationLevelResponse> UpdateEducationLevel(long id, EducationLevelRequest EducationLevelRequest);
            ApiResponse<object> DeleteEducationLevel(long id);
        }
}
