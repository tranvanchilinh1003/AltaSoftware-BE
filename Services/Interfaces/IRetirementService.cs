using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IRetirementService
    {
        ApiResponse<ICollection<RetirementResponse>> GetRetirement(int page, int pageSize, string search, string sortColumn, string sortOrder);

        ApiResponse<ICollection<RetirementResponse>> GetRetirementNoPaging();
        ApiResponse<RetirementResponse> GetRetirementById(long id);
        ApiResponse<ICollection<RetirementResponse>> GetRetirementByTeacherId(long id);
        ApiResponse<RetirementResponse> CreateRetirement(RetirementRequest Retirement_AddRequest);
        ApiResponse<Retirement> UpdateRetirement(long id, RetirementRequest Retirement_UpdateRequest);
        ApiResponse<Retirement> UpdateRetirementByTeacherId(long id, RetirementRequest Retirement_UpdateRequest);
        ApiResponse<Retirement> DeleteRetirement(long id);
    }

}
