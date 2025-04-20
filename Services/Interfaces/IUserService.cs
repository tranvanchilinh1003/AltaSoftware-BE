using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<ICollection<UserResponse>>> GetUsers(int page, int pageSize, string search, string sortColumn, string sortOrder);
        Task<ApiResponse<UserResponse>> GetUserById(int id);
        Task<ApiResponse<UserResponse>> GetUserByCode(string code);
        ApiResponse<int> GetQuantityUserByRoleId(int roleId);
        ApiResponse<UserResponse> CreateUser(UserRequest userRequest);
        ApiResponse<UserResponse> UpdateUser(int id, UserUpdateRequest userUpdateRequest);
        ApiResponse<User> DeleteUser(int id);
        ApiResponse<UserResponse> UpdateUserPassword(int userId, string newPassword);
        ApiResponse<UserResponse> ChangePassword(int userId, string currentPassword, string newPassword);
        Task<ApiResponse<StudentProcessResponse>> GetStudentById(int userId);
    }
}
