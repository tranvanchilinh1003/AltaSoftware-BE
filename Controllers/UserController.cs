using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<ApiResponse<ICollection<UserResponse>>>> GetUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc")
        {
            var result = await _userService.GetUsers(page, pageSize, search, sortColumn, sortOrder);
            return Ok(result);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(result);
        }

        // POST: api/users
        [HttpPost]
        public ActionResult<ApiResponse<UserResponse>> CreateUser([FromBody] UserRequest userRequest)
        {
            if (userRequest == null)
                return BadRequest("Dữ liệu người dùng không hợp lệ.");

            userRequest.Dob = DateTimeUtils.ConvertToUnspecified(userRequest.Dob);
            userRequest.EnrollmentDate = DateTimeUtils.ConvertToUnspecified(userRequest.EnrollmentDate);

            return Ok(_userService.CreateUser(userRequest));
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<UserResponse>> UpdateUser(int id, [FromBody] UserUpdateRequest userRequest)
        {
            if (userRequest == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            // Chuyển đổi DateTime thành Unspecified trước khi cập nhật
            userRequest.Dob = DateTimeUtils.ConvertToUnspecified(userRequest.Dob);
            userRequest.EnrollmentDate = DateTimeUtils.ConvertToUnspecified(userRequest.EnrollmentDate);

            // Gửi ID và request đến service
            return Ok(_userService.UpdateUser(id, userRequest));
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<User>> DeleteUser(int id)
        {
            return Ok(_userService.DeleteUser(id));
        }

        [HttpPost("GetQuantityUserByRoleId/{roleId}")]

        public ActionResult<ApiResponse<int>> GetQuantityUserByRoleId(int roleId)
        {
            var response = _userService.GetQuantityUserByRoleId(roleId);

            return response.Code == 0 ? Ok(response) : BadRequest(response); ;
        }

        //
        [HttpGet("student/learning-process")]
        public async Task<IActionResult> GetStudentById([FromQuery] int userId)
        {
            var response = await _userService.GetStudentById(userId);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
