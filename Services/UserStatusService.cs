using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public class UserStatusService : IUserStatusService
    {
        private readonly UserStatusRepo _repository;
        private readonly IMapper _mapper;

        public UserStatusService(UserStatusRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<UserStatusResponse>> GetUserStatuses(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetUserStatuses().Where(us => !us.IsDeleted).AsQueryable();

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
            var response = _mapper.Map<ICollection<UserStatusResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<UserStatusResponse>>.Success(response)
                : ApiResponse<ICollection<UserStatusResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<UserStatusResponse> GetUserStatusById(long id)
        {
            var userStatus = _repository.GetUserStatusById(id);
            if (userStatus == null || userStatus.IsDeleted)
                return ApiResponse<UserStatusResponse>.NotFound($"Không tìm thấy trạng thái người dùng #{id}");

            return ApiResponse<UserStatusResponse>.Success(_mapper.Map<UserStatusResponse>(userStatus));
        }

        public ApiResponse<UserStatusResponse> CreateUserStatus(UserStatusRequest userStatusRequest)
        {
            var existing = _repository.GetUserStatuses().FirstOrDefault(us => us.Name.ToLower() == userStatusRequest.Name.ToLower() && !us.IsDeleted);
            if (existing != null)
            {
                return ApiResponse<UserStatusResponse>.Conflict("Tên trạng thái đã tồn tại");
            }

            var created = _repository.CreateUserStatus(new UserStatus { Name = userStatusRequest.Name });
            return ApiResponse<UserStatusResponse>.Success(_mapper.Map<UserStatusResponse>(created));
        }

        public ApiResponse<UserStatusResponse> UpdateUserStatus(long id, UserStatusRequest userStatusRequest)
        {
            var userStatus = _repository.GetUserStatusById(id);
            if (userStatus == null || userStatus.IsDeleted)
                return ApiResponse<UserStatusResponse>.NotFound("Không tìm thấy trạng thái người dùng để cập nhật");

            userStatus.Name = userStatusRequest.Name;
            var updated = _repository.UpdateUserStatus(userStatus);

            return ApiResponse<UserStatusResponse>.Success(_mapper.Map<UserStatusResponse>(updated));
        }

        public ApiResponse<bool> DeleteUserStatus(long id)
        {
            var userStatus = _repository.GetUserStatusById(id);
            if (userStatus == null || userStatus.IsDeleted)
                return ApiResponse<bool>.NotFound("Không tìm thấy trạng thái người dùng để xóa");

            userStatus.IsDeleted = true;
            _repository.UpdateUserStatus(userStatus);

            return ApiResponse<bool>.Success();
        }
    }
}
