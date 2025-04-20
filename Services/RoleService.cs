using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleRepo _repository;
        private readonly IMapper _mapper;

        public RoleService(RoleRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public ApiResponse<ICollection<RoleResponse>> GetRoles(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetRoles().AsQueryable();

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

            var response = _mapper.Map<ICollection<RoleResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<RoleResponse>>.Success(response)
                : ApiResponse<ICollection<RoleResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<RoleResponse> GetRoleById(long id)
        {
            var Role = _repository.GetRoleById(id);
            return Role != null
                ? ApiResponse<RoleResponse>.Success(_mapper.Map<RoleResponse>(Role))
                : ApiResponse<RoleResponse>.NotFound($"Không tìm thấy #{id}");
        }

        public ApiResponse<RoleResponse> GetRoleByName(string name)
        {
            var role = _repository.GetRoles().FirstOrDefault(us => us.Name?.ToLower() == name.ToLower());
            return role != null
                ? ApiResponse<RoleResponse>.Success(_mapper.Map<RoleResponse>(role))
                : ApiResponse<RoleResponse>.NotFound($"Không tìm thấy tên: {name}");
        }

        public ApiResponse<RoleResponse> CreateRole(RoleRequest roleRequest)
        {
            var existing = _repository.GetRoles().FirstOrDefault(us => us.Name?.ToLower() == roleRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<RoleResponse>.Conflict("Tên đã tồn tại");
            }

            var created = _repository.CreateRole(new Role() { Name = roleRequest.Name, Description = roleRequest.Description });
            return ApiResponse<RoleResponse>.Success(_mapper.Map<RoleResponse>(created));
        }

        public ApiResponse<Role> UpdateRole(long id ,RoleRequest roleRequest)
        {
            var updated = _repository.UpdateRole(id, roleRequest);
            return updated != null
                ? ApiResponse<Role>.Success(updated)
                : ApiResponse<Role>.NotFound("Không tìm thấy vai trò để sửa");
        }

        public ApiResponse<Role> DeleteRole(long id)
        {
            var success = _repository.DeleteRole(id);
            return success
                ? ApiResponse<Role>.Success()
                : ApiResponse<Role>.NotFound("Không tìm thấy vai trò để xóa");
        }
    }
}
