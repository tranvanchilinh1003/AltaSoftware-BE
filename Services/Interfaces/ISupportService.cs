using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ISupportService
    {
        ApiResponse<ICollection<SupportResponse>> GetSupports(int? page, int? pageSize, string? sortColumn, string? sortOrder);
        ApiResponse<SupportResponse> GetSupportById(int id);
        ApiResponse<SupportResponse> CreateSupport(SupportRequest SupportRequest);
        ApiResponse<SupportResponse> UpdateSupport(int id, SupportRequest SupportRequest);
        ApiResponse<bool> DeleteSupport(int id);
    }
}
