using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/test-submission-answer")]
    [ApiController]
    [Authorize]
    public class TestSubmissionAnswerController : Controller
    {
        private readonly ITestSubmissionAnswerService _service;

        public TestSubmissionAnswerController(ITestSubmissionAnswerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTestSubmissionAnswers
        (
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc"
        )
        {
            //Console.WriteLine($"API called with params - Page: {page}, PageSize: {pageSize}, Search: {search}, SortColumn: {sortColumn}, SortOrder: {sortOrder}");
            var response = _service.GetTestSubmissionAnswers(page, pageSize, search, sortColumn, sortOrder);
            //Console.WriteLine($"API response: {response.Data.Count} records returned");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTestSubmissionAnswerById(int id)
        {
            var response = _service.GetTestSubmissionAnswerById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("by-test/{testId}")]
        public IActionResult GetByTest(long testId, int pageNumber = 1, int pageSize = 10)
        {
            var result = _service.GetAnswersByTestId(testId, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestSubmissionAnswer(
        [FromForm] TestSubmissionAnswerRequest request,
        [FromForm] List<IFormFile> attachments)
        {
            try
            {
                // Gọi service để xử lý file và dữ liệu
                var response = await _service.CreateTestSubmissionAnswer(request, attachments);

                if (response.Code == 0) // Success
                    return Ok(response);

                if (response.Message.Contains("Conflict"))
                    return Conflict(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<TestSubmissionAnswerResponse>(1, "Lỗi hệ thống", null, new Dictionary<string, string[]>
                {
                    { "Exception", new[] { ex.Message } }
                }));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestSubmissionAnswer(
            int id,
            [FromForm] TestSubmissionAnswerRequest request,
            [FromForm] List<IFormFile> attachments)
        {
            var response = await _service.UpdateTestSubmissionAnswer(id, request, attachments);

            if (response.Code == 0)
                return Ok(response);

            if (response.Message.Contains("Conflict"))
                return Conflict(response);

            return BadRequest(response);
        }


        [HttpPut("{id}/toggle-active")]
        public IActionResult DeleteTestSubmissionAnswer(int id)
        {
            var response = _service.DeleteTestSubmissionAnswer(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
