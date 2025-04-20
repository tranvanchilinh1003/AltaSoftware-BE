using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IChangeClassService
    {
        ApiResponse<ICollection<ChangeClassResponse>> GetChangeClasses(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ICollection<ChangeClassResponse>> GetChangeClassesNormal();
        ApiResponse<ChangeClassResponse> GetChangeClassById(long id);
        ApiResponse<ChangeClassResponse> CreateChangeClass(ChangeClass_AddRequest ChangeClassRequest);
        ApiResponse<ChangeClass> UpdateChangeClass(long id, ChangeClass_UpdateRequest ChangeClass);
        ApiResponse<ChangeClass> DeleteChangeClass(long id);
    }


}
