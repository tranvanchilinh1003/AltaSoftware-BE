using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/tests-attachments")]
    public class TestsAttachmentController : Controller
    {
        private readonly ITestsAttachmentService _service;
        public TestsAttachmentController(ITestsAttachmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllTestsAttachments
        (
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc"
        )
        {
            var response = _service.GetTestsAttachments(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpPost]
        public  IActionResult CreateAttachment([FromBody] TestsAttachmentRequest request)
        {
            var response =  _service.CreateTestsAttachment(request);

            if (response.Code != 0)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTestsAttachment(long id, [FromBody] TestsAttachmentRequest request)
        {
            var response = _service.UpdateTestsAttachment(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var response = _service.GetTestsAttachmentById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPut("{id}/toggle-active")]
        public IActionResult DeleteTestsAttachment(long id)
        {
            var response = _service.DeleteTestsAttachment(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
