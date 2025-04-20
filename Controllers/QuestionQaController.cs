using System.Security.Claims;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/question-qa")]
    public class QuestionQaController : ControllerBase
    {
        private readonly IQuestionQaService _service;

        public QuestionQaController(IQuestionQaService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetQuestions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "Id",
            [FromQuery] string sortOrder = "asc",
            [FromQuery] int? classId = null,
            [FromQuery] int? subjectId = null)
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

            if (!classId.HasValue || !subjectId.HasValue)
            {
                return BadRequest("Thiếu ClassId hoặc SubjectId");
            }

            var response = _service.GetQuestions(userId, page, pageSize, search, sortColumn, sortOrder, classId, subjectId);
            return Ok(response);
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromForm] QuestionQaRequest questionRequest, [FromForm] List<IFormFile> files)
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

            var response = await _service.CreateQuestion(questionRequest, files, userId);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }



        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(long id)
        {
            var response = _service.DeleteQuestion(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }


        
        [Authorize]
        [HttpGet("answered")]
        public IActionResult GetAnsweredQuestions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? classId = null,
            [FromQuery] int? subjectId = null)
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
            if (!classId.HasValue || !subjectId.HasValue)
            {
                return BadRequest("Thiếu ClassId hoặc SubjectId");
            }
            var response = _service.GetAnsweredQuestions(userId, page, pageSize, classId, subjectId);
            return Ok(response);
        }
            
            [Authorize]
            [HttpGet("search")]
            public IActionResult SearchQuestionsByUserName(
                [FromQuery] string userName,
                [FromQuery] bool onlyAnswered = false,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10,
                [FromQuery] int? classId = null,
                [FromQuery] int? subjectId = null)
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
            if (!classId.HasValue || !subjectId.HasValue)
            {
                return BadRequest("Thiếu ClassId hoặc SubjectId");
            }
                var response = _service.SearchQuestionsByUserName(userId, userName, onlyAnswered, page, pageSize, classId, subjectId);
                return Ok(response);
            }

        [Authorize]
        [HttpGet("recent")]
        public IActionResult GetRecentQuestions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? classId = null,
            [FromQuery] int? subjectId = null)
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
            if (!classId.HasValue || !subjectId.HasValue)
            {
                return BadRequest("Thiếu ClassId hoặc SubjectId");
            }
            var response = _service.GetRecentQuestions(userId, page, pageSize, classId, subjectId);
            return Ok(response);
        }



        [HttpPut("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult UpdateQuestion(long id, [FromBody] QuestionQaRequest question)
        {
            var response = _service.UpdateQuestion(id, question);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("{idqs}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetQuestionById(long id)
        {
            var response = _service.GetQuestionById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("{idus}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetQuestionById(int id, [FromQuery] int userId)
        {
            var response = _service.GetQuestionByIdForUser(id, userId);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
    
}
