using System.Security.Claims;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/answers-qa")]
    public class AnswersQaController : ControllerBase
    {
        private readonly IAnswersQaService _service;

        public AnswersQaController(IAnswersQaService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAnswers([FromQuery] long? questionId)
        {   
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
           if (userIdClaim == null)
            {
                return Unauthorized(ApiResponse<string>.Unauthorized("Không có quyền truy cập. Vui lòng đăng nhập."));
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(ApiResponse<string>.Unauthorized("Token không chứa thông tin userId hợp lệ."));
            }

            var response = _service.GetAnswers(questionId);
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateAnswer([FromForm] AnswersQaRequest answerRequest)
        {
            // Lấy userId từ token theo cách bạn yêu cầu
             var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

           if (userIdClaim == null)
            {
                return Unauthorized(ApiResponse<string>.Unauthorized("Không có quyền truy cập. Vui lòng đăng nhập."));
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(ApiResponse<string>.Unauthorized("Token không chứa thông tin userId hợp lệ."));
            }

            //Truyền userId vào service
            var response = await _service.CreateAnswer(answerRequest, userId);
            return Ok(response);
        }


        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteAnswer(long id)
        {
            var response = _service.DeleteAnswer(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }


        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetAnswerById(long id)
        {
            var response = _service.GetAnswerById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPut("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public IActionResult UpdateAnswer(long id, [FromBody] AnswersQaRequest answerRequest)
        {
            var response = _service.UpdateAnswer(id, answerRequest);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }



    }
}
