using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Security;

namespace ISC_ELIB_SERVER.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly RolePermissionRepo _repository;
        private readonly IMapper _mapper;

        public RolePermissionService(RolePermissionRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<RolePermissionResponse> CreateRolePermission(RolePermissionRequest rolePermissionRequest)
        {
            var existing = _repository.GetRolePermissions().FirstOrDefault(rps => rps.RoleId == rolePermissionRequest.RoleId && rps.PermissionId == rolePermissionRequest.PermissionId);
            if (existing != null)
            {
                return ApiResponse<RolePermissionResponse>.Conflict("Đã tồn tại");
            }

            var created = _repository.CreateRolePermission(new RolePermission() { RoleId = rolePermissionRequest.RoleId, PermissionId = rolePermissionRequest.PermissionId });
            return ApiResponse<RolePermissionResponse>.Success(_mapper.Map<RolePermissionResponse>(created));
        }

        public ApiResponse<RolePermission> DeleteRolePermission(long id)
        {
            var success = _repository.DeleteRolePermission(id);
            return success
                ? ApiResponse<RolePermission>.Success()
                : ApiResponse<RolePermission>.NotFound("Không tìm thấy để xóa");
        }

        public ApiResponse<RolePermissionResponse> GetRolePermissionById(long id)
        {
            var rolePermission = _repository.GetRolePermissionById(id);
            return rolePermission != null
                ? ApiResponse<RolePermissionResponse>.Success(_mapper.Map<RolePermissionResponse>(rolePermission))
                : ApiResponse<RolePermissionResponse>.NotFound($"Không tìm thấy #{id}");
        }

        public ApiResponse<ICollection<RolePermissionResponse>> GetRolePermissions(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetRolePermissions().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Id.ToString().ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<RolePermissionResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<RolePermissionResponse>>.Success(response)
                : ApiResponse<ICollection<RolePermissionResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<RolePermission> UpdateRolePermission(long id, RolePermissionRequest rolePermissionRequest)
        {
            var updated = _repository.UpdateRolePermission(id , rolePermissionRequest);
            return updated != null
                ? ApiResponse<RolePermission>.Success(updated)
                : ApiResponse<RolePermission>.NotFound("Không tìm thấy quyền để sửa");
        }
    }
}
