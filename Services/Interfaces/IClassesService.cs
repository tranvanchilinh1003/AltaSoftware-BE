using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services
{
    public interface IClassesService
    {
        ApiResponse<ICollection<ClassesResponse>> GetClass(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<ICollection<ClassesResponse>> GetClassByGradeLevelIdAndAcademicYearId(int? page, int? pageSize, int? gradeLevelId, int? academicYearId, string? sortColumn, string? sortOrder);
        ApiResponse<ClassesResponse> GetClassById(int id);
        ApiResponse<bool> DeleteClass(List<int> ids);

        Task<ApiResponse<bool>> UpdateClassSubjectsAsync(int classId, List<int> subjectIds);
        Task<ApiResponse<ClassesResponse>> CreateClassAsync(ClassesRequest classesRequest);
        Task<ApiResponse<ClassesResponse>> UpdateClassAsync(int id, ClassesRequest classesRequest);

        Task<ApiResponse<bool>> ImportClassesAsync(IFormFile file);

        Task<ApiResponse<bool>> UpdateClassUserStatus(int classId, int userId, int newStatusId);

        ApiResponse<ICollection<ClassesResponse>> GetClassBySubjectId(int? page, int? pageSize, int?subjectId, string? sortColumn, string? sortOrder);

        ApiResponse<ICollection<ClassesResponse>> GetClassByCoBan(int? page, int? pageSize, string? sortColumn, string? sortOrder);

        ApiResponse<ICollection<ClassesResponse>> GetClassByNangCao(int? page, int? pageSize, string? sortColumn, string? sortOrder);

    }
}
