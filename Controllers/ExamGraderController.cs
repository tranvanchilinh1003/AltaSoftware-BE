using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/exam-graders")]
    public class ExamGraderController : ControllerBase
    {
        private readonly IExamGraderService _service;

        public ExamGraderController(IExamGraderService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll(
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10,
     [FromQuery] string? search = null,
     [FromQuery] string? sortBy = "Id",
     [FromQuery] bool isDescending = false)
        {
            var response = _service.GetAll(page, pageSize, search, sortBy, isDescending);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var response = _service.GetById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ExamGraderRequest request)
        {
            var response = _service.Create(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ExamGraderRequest request)
        {
            var response = _service.Update(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var response = _service.Delete(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
