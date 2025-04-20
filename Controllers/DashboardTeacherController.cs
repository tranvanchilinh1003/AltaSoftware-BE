using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISC_ELIB_SERVER.Controllers
{
    [Authorize] // Yêu cầu đăng nhập
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardTeacherController : ControllerBase
    {
        private readonly IDashboardTeacherService _dashboardTeacherService;
        private readonly IUserService _userService;

        public DashboardTeacherController(IDashboardTeacherService dashboardTeacherService, IUserService userService)
        {
            _dashboardTeacherService = dashboardTeacherService;
            _userService = userService;
        }

        // API lấy tổng quan dashboard theo user đăng nhập
        [HttpGet("overview")]
        public IActionResult GetDashboardOverview()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(ApiResponse<string>.Fail("Không tìm thấy ID trong token"));

            var response = _dashboardTeacherService.GetDashboardOverview(userId.Value);
            return Ok(response);
        }

        // API lấy thống kê học sinh theo user đăng nhập
        [HttpGet("student-statistics")]
        public IActionResult GetStudentStatistics()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(ApiResponse<string>.Fail("Không tìm thấy ID trong token"));

            var response = _dashboardTeacherService.GetStudentStatistics(userId.Value);
            return Ok(response);
        }

        // Lấy userId từ token JWT
        private int? GetUserId()
        {
            var userIdString = User.FindFirst("Id")?.Value;
            Console.WriteLine($"User.FindFirst(\"Id\"): {userIdString}");

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return null; // Trả về null nếu không tìm thấy hoặc parse thất bại
            }

            return userId;
        }

    }
}

