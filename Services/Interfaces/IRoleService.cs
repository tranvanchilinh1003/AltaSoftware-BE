using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IRoleService
    {
        ApiResponse<ICollection<RoleResponse>> GetRoles(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<RoleResponse> GetRoleById(long id);
        ApiResponse<RoleResponse> GetRoleByName(string name);
        ApiResponse<RoleResponse> CreateRole(RoleRequest roleRequest);
        ApiResponse<Role> UpdateRole(long id, RoleRequest roleRequest);
        ApiResponse<Role> DeleteRole(long id);
    }
}
