using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IPermissionService
    {
        ApiResponse<ICollection<PermissionResponse>> GetPermissions(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<PermissionResponse> GetPermissionById(long id);
        ApiResponse<PermissionResponse> GetPermissionByName(string name);
        ApiResponse<PermissionResponse> CreatePermission(PermissionRequest permissionRequest);
        ApiResponse<Permission> UpdatePermission(long id, PermissionRequest permissionRequest);
        ApiResponse<Permission> DeletePermission(long id);
    }

}
