using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ISubjectGroupService
    {
        ApiResponse<ICollection<SubjectGroupResponse>> GetSubjectGroup(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<SubjectGroupResponse> GetSubjectGroupById(long id);
        ApiResponse<SubjectGroupResponse> CreateSubjectGroup(SubjectGroupRequest request);
        ApiResponse<SubjectGroupResponse> UpdateSubjectGroup(long id, SubjectGroupRequest request);
        ApiResponse<string> DeleteSubjectGroup(long id);
    }
}
