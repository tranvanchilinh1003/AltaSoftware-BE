using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ISubjectTypeService
    {
        ApiResponse<ICollection<SubjectTypeResponse>> GetSubjectType(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<SubjectTypeResponse> GetSubjectTypeById(long id);
        ApiResponse<SubjectTypeResponse> CreateSubjectType(SubjectTypeRequest subjectTypeRequest);
        ApiResponse<SubjectTypeResponse> UpdateSubjectType(long id, SubjectTypeRequest subjectTypeRequest);
        ApiResponse<string> DeleteSubjectType(long id);
    }
}
