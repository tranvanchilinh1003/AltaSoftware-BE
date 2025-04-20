using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IRolePermissionService
    {
        ApiResponse<ICollection<RolePermissionResponse>> GetRolePermissions(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<RolePermissionResponse> GetRolePermissionById(long id);
        ApiResponse<RolePermissionResponse> CreateRolePermission(RolePermissionRequest rolePermissionRequest);
        ApiResponse<RolePermission> UpdateRolePermission(long id, RolePermissionRequest rolePermissionRequest);
        ApiResponse<RolePermission> DeleteRolePermission(long id);
    }
}
