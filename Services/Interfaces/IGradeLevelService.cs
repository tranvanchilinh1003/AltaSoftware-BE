using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IGradeLevelService
    {
        ApiResponse<ICollection<GradeLevelResponse>> GetGradeLevels(int? page, int? pageSize, string? sortColumn, string? sortOrder);
        ApiResponse<ICollection<GradeLevelResponse>> GetGradeLevelsByAyAndSc(int? page, int? pageSize, string? sortColumn, string? sortOrder, string schoolName, int? startYear, int? endYear);
        ApiResponse<GradeLevelResponse> GetGradeLevelById(long id);
        ApiResponse<object> GetClassOfGradeLevel(long id);
        ApiResponse<GradeLevelResponse> CreateGradeLevel(GradeLevelRequest GradeLevelRequest);
        ApiResponse<GradeLevelResponse> UpdateGradeLevel(long id, GradeLevelRequest GradeLevelRequest);
        ApiResponse<object> DeleteGradeLevel(long id);
    }
}
