using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/question-view")]
    public class QuestionViewController : ControllerBase
    {
        private readonly IQuestionViewService _service;

        public QuestionViewController(IQuestionViewService service)
        {
            _service = service;
        }

        // API lấy số lượt xem của câu hỏi
        [Authorize]
        [HttpGet("{questionId}")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public IActionResult GetViewCount(int questionId)
        {
            var response = _service.GetViewCount(questionId);
            return Ok(response);
        }

        // API thêm lượt xem
        [Authorize]
        [HttpPost]
        public IActionResult AddView([FromBody] QuestionViewRequest request)
        {
            // 👇 Lấy userId từ JWT token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

           if (userIdClaim == null)
            {
                return Unauthorized(ApiResponse<string>.Unauthorized("Không có quyền truy cập. Vui lòng đăng nhập."));
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(ApiResponse<string>.Unauthorized("Token không chứa thông tin userId hợp lệ."));
            }

            // 👇 Gọi service và truyền userId riêng
            _service.AddView(request.QuestionId, userId);

            return Ok(new { message = "Lượt xem đã được cập nhật." });
        }
    }
}
