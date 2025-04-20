using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _service;

        public TestController(ITestService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTestes([FromQuery] int? page = null, [FromQuery] int? pageSize = null,
            [FromQuery] string? search = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortOrder = null)
        {
            var response = _service.GetTestes(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("get-by-students")]
        public IActionResult GetTestesByStudent([FromQuery] int? page = null, [FromQuery] int? pageSize = null,
            [FromQuery] string? search = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortOrder = null,
            [FromQuery] int status = 0, [FromQuery] long? subjectGroupId = null, [FromQuery] long? gradeLevelsId = null,
            [FromQuery] string? date = null)
        {
            var userId = User.FindFirst("Id")?.Value;
            var response = _service.GetTestesByStudent(page, pageSize, search, sortColumn, sortOrder, status, subjectGroupId, gradeLevelsId, date, userId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTestById(long id)
        {
            var response = _service.GetTestById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateTest([FromBody] TestRequest TestRequest)
        {
            var userId = User.FindFirst("Id")?.Value;

            var response = _service.CreateTest(TestRequest, userId);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTest(long id, [FromBody] TestRequest Test)
        {
            var userId = User.FindFirst("Id")?.Value;
            
            var response = _service.UpdateTest(id,Test, userId);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTest(long id)
        {
            var response = _service.DeleteTest(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

    }
}
