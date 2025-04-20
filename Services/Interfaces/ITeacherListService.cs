using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITeacherListService
    {
        ApiResponse<ICollection<TeacherListResponse>> GetTeacherLists(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<TeacherListResponse> GetTeacherListById(int id);
    }
}
