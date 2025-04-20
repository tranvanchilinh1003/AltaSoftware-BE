using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IMajorService
    {
        ApiResponse<ICollection<MajorResponse>> GetMajor(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<MajorResponse> GetMajorById(long id);
        ApiResponse<MajorResponse> CreateMajor(MajorRequest majorRequest);
        ApiResponse<MajorResponse> UpdateMajor(long id, MajorRequest majorRequest);
        ApiResponse<Major> DeleteMajor(long id);
    }
}
