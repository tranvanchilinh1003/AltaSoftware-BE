using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public interface IUserStatusService
    {
        ApiResponse<ICollection<UserStatusResponse>> GetUserStatuses(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<UserStatusResponse> GetUserStatusById(long id);
        ApiResponse<UserStatusResponse> CreateUserStatus(UserStatusRequest userStatusRequest);
        ApiResponse<UserStatusResponse> UpdateUserStatus(long id, UserStatusRequest userStatusRequest);
        ApiResponse<bool> DeleteUserStatus(long id);
    }
}
