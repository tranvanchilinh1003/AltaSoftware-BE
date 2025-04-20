using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/test-submission")]
    [ApiController]
    public class TestsSubmissionController : ControllerBase
    {
        private readonly ITestsSubmissionService _service;

        public TestsSubmissionController(ITestsSubmissionService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTestes([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetTestsSubmissiones(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTestById(long id)
        {
            var response = _service.GetTestsSubmissionById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("by-test/{testId}")]
        public async Task<IActionResult> GetSubmissionsByTestId(int testId)
        {
            var result = await _service.GetByTestIdAsync(testId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestsSubmission(
            [FromForm] TestsSubmissionRequest request,
            [FromForm] List<TestSubmissionAnswerRequest>? answerRequests)
        {
            var result = await _service.CreateTestsSubmission(request, answerRequests);
            return Ok(result);
        }

        [HttpPut("{submissionId}")]
        public async Task<IActionResult> UpdateTest(
                int submissionId, [FromForm] TestsSubmissionRequest request, [FromForm] List<TestSubmissionAnswerRequest>? answerRequests)
        {
            var result = await _service.UpdateTestsSubmission(submissionId, request, answerRequests);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTest(long id)
        {
            var response = _service.DeleteTestsSubmission(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

    }
}
