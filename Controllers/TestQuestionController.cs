using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/test-question")]
    public class TestQuestionController : ControllerBase
    {
        private readonly ITestQuestionService _service;

        public TestQuestionController(ITestQuestionService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTestes(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", 
            [FromQuery] string sortColumn = "Id", 
            [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetTestQuestiones(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTestById(long id)
        {
            var response = _service.GetTestQuestionById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateTest([FromBody] TestQuestionRequest TestRequest)
        {
            var response = _service.CreateTestQuestion(TestRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTest(long id, [FromBody] TestQuestionRequest Test)
        {
            var response = _service.UpdateTestQuestion(id, Test);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTest(long id)
        {
            var response = _service.DeleteTestQuestion(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

      


    }
}
