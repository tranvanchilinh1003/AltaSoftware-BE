using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
public interface ICampusService
{
    ApiResponse<ICollection<CampusResponse>> GetCampuses(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
    ApiResponse<CampusResponse> GetCampusById(long id);
    ApiResponse<CampusResponse> CreateCampus(CampusRequest campusRequest);
    ApiResponse<CampusResponse> UpdateCampus(long id, CampusRequest campusRequest);
    ApiResponse<bool> DeleteCampus(long id);
}
