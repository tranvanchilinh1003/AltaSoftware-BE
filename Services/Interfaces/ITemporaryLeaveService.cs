using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITemporaryLeaveService
    {
        ApiResponse<ICollection<TemporaryLeaveResponse>> GetTemporaryLeaves(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ICollection<TemporaryLeaveResponse>> GetTemporaryLeavesNormal();
        ApiResponse<TemporaryLeaveResponse> GetTemporaryLeaveById(long id);
        ApiResponse<TemporaryLeaveResponse> CreateTemporaryLeave(TemporaryLeave_AddRequest TemporaryLeaveRequest);
        ApiResponse<TemporaryLeave> UpdateTemporaryLeave(long id, TemporaryLeave_UpdateRequest TemporaryLeave);
        ApiResponse<TemporaryLeave> DeleteTemporaryLeave(long id);
    }

}
