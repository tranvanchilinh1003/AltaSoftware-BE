using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/exam")]
    [ApiController]
    public class ExamController : Controller
    {
        private readonly IExamService _service;
        public ExamController(IExamService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllExams
        (
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc"
        )
        {
            var response = _service.GetExams(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id:long}")]
        public IActionResult GetExamById(long id)
        {
            var response = _service.GetExamById(id);
            if (response.Code == 0)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("{name}")]
        public IActionResult GetExamByName(string name)
        {
            var response = _service.GetExamByName(name);
            if (response.Code == 0)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateExam([FromForm] ExamRequest request)
        {
            var response = _service.CreateExam(request);
            if (response.Code == 0)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateExam(long id, [FromForm] ExamRequest request)
        {
            var response = _service.UpdateExam(id, request);
            if (response.Code == 0)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}/toggle-active")]
        public IActionResult DeleteExam(long id)
        {
            var response = _service.DeleteExam(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
