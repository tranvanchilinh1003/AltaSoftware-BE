using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/quiz")]
    [ApiController]
    public class TestAnswerController : ControllerBase
    {
        private readonly TestAnswerService _service;

        public TestAnswerController(TestAnswerService service)
        {
            _service = service;
        }

        //  Lấy danh sách câu trả lời theo QuestionId
        [Authorize]
        [HttpGet("by-question/{questionId}")]
        public IActionResult GetAnswersByQuestion(int questionId)
        {
            var response = _service.GetAnswersByQuestion(questionId);
            return Ok(response);
        }
        
        //  Tạo câu trả lời
        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateTestAnswer([FromForm] TestAnswerRequest request)
        {
            var result = _service.CreateTestAnswer(request);
            return Ok(result);
        }

        //  Cập nhật câu trả lời
        [Authorize]
        [HttpPut("update/{id}")]
        public IActionResult UpdateTestAnswer(int id, [FromForm] TestAnswerRequest request)
        {
            var result = _service.UpdateTestAnswer(id, request);
            return Ok(result);
        }

        //  Xóa câu trả lời
        [Authorize]
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteTestAnswer(int id)
        {
            var result = _service.DeleteTestAnswer(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("import-excel")]
        public async Task<IActionResult> ImportExcel([FromForm] ImportQuestionExcelRequest request)
        {
            var result = await _service.ImportFromExcelAsync(request.File, request.TestId, request.QuestionType);
            return Ok(result);
        }

    }
}
