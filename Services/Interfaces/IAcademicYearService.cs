using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
public interface IAcademicYearService
{
    ApiResponse<ICollection<AcademicYearResponse>> GetAcademicYears(int? page, int? pageSize, string? sortColumn, string? sortOrder);
    ApiResponse<AcademicYearResponse> GetAcademicYearById(long id);
    Task<ApiResponse<AcademicYearResponse>> CreateAcademicYear(AcademicYearRequest academicYearRequest);
    ApiResponse<AcademicYearResponse> UpdateAcademicYear(long id, ICollection<AcademicYearSemesterRequest> academicYearRequest);
    ApiResponse<object> DeleteAcademicYear(long id);
}
