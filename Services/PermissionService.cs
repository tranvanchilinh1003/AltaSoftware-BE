using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Security;

namespace ISC_ELIB_SERVER.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly PermissionRepo _repository;
        private readonly IMapper _mapper;

        public PermissionService(PermissionRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public ApiResponse<ICollection<PermissionResponse>> GetPermissions(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetPermissions().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<PermissionResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<PermissionResponse>>.Success(response)
                : ApiResponse<ICollection<PermissionResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<PermissionResponse> GetPermissionById(long id)
        {
            var permission = _repository.GetPermissionById(id);
            return permission != null
                ? ApiResponse<PermissionResponse>.Success(_mapper.Map<PermissionResponse>(permission))
                : ApiResponse<PermissionResponse>.NotFound($"Không tìm thấy #{id}");
        }

        public ApiResponse<PermissionResponse> GetPermissionByName(string name)
        {
            var Permission = _repository.GetPermissions().FirstOrDefault(us => us.Name?.ToLower() == name.ToLower());
            return Permission != null
                ? ApiResponse<PermissionResponse>.Success(_mapper.Map<PermissionResponse>(Permission))
                : ApiResponse<PermissionResponse>.NotFound($"Không tìm thấy tên: {name}");
        }

        public ApiResponse<PermissionResponse> CreatePermission(PermissionRequest permissionRequest)
        {
            var existing = _repository.GetPermissions().FirstOrDefault(us => us.Name?.ToLower() == permissionRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<PermissionResponse>.Conflict("Tên đã tồn tại");
            }

            var created = _repository.CreatePermission(new Permission() { Name = permissionRequest.Name });
            return ApiResponse<PermissionResponse>.Success(_mapper.Map<PermissionResponse>(created));
        }

        public ApiResponse<Permission> UpdatePermission(long id, PermissionRequest permissionRequest)
        {
            var updated = _repository.UpdatePermission(id ,permissionRequest);
            return updated != null
                ? ApiResponse<Permission>.Success(updated)
                : ApiResponse<Permission>.NotFound("Không tìm thấy quyền để sửa");
        }

        public ApiResponse<Permission> DeletePermission(long id)
        {
            var success = _repository.DeletePermission(id);
            return success
                ? ApiResponse<Permission>.Success()
                : ApiResponse<Permission>.NotFound("Không tìm thấy quyền để xóa");
        }
    }
}
