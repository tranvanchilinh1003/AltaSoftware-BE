using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IExemptionService
    {
        ApiResponse<ICollection<ExemptionResponse>> GetExemptions(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ICollection<ExemptionResponse>> GetExemptionsNormal();
        ApiResponse<ExemptionResponse> GetExemptionById(long id);
        ApiResponse<ExemptionResponse> CreateExemption(Exemption_AddRequest ExemptionRequest);
        ApiResponse<Exemption> UpdateExemption(long id, Exemption_UpdateRequest Exemption);
        ApiResponse<Exemption> DeleteExemption(long id);
    }

}
