using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IWorkProcessService
    {
        ApiResponse<ICollection<WorkProcessResponse>> GetWorkProcess(int page, int pageSize, string search, string sortColumn, string sortOrder);

        ApiResponse<ICollection<WorkProcessResponse>> GetWorkProcessNoPaging();
        ApiResponse<WorkProcessResponse> GetWorkProcessById(long id);
        ApiResponse<ICollection<WorkProcessResponse>> GetWorkProcessByTeacherId(long id);
        ApiResponse<WorkProcessResponse> CreateWorkProcess(WorkProcessRequest workProcess_AddRequest);
        ApiResponse<WorkProcess> UpdateWorkProcess(long id, WorkProcessRequest workProcess_UpdateRequest);
        ApiResponse<WorkProcess> DeleteWorkProcess(long id);
    }
}
