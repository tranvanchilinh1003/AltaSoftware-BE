using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScheduleController : ControllerBase
    {
        private readonly IExamScheduleService _service;

        public ExamScheduleController(IExamScheduleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll(
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10,
     [FromQuery] string? search = null,
     [FromQuery] string? sortBy = "Id",
     [FromQuery] bool isDescending = false,
     [FromQuery] int? academicYearId = null,
     [FromQuery] int? semesterId = null,
     [FromQuery] int? gradeLevelsId = null, 
     [FromQuery] int? classId = null       
 )
        {
            var response = _service.GetAll(page, pageSize, search, sortBy, isDescending, academicYearId, semesterId, gradeLevelsId, classId);
            return StatusCode(response.Code == 0 ? 200 : 400, response);
        }

        [HttpGet("by-academic-year")]
        public IActionResult GetByAcademicYear(
    [FromQuery] int academicYearId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10
)
        {
            var response = _service.GetAll(
                page,
                pageSize,
                null,         // search
                "Id",         // sortBy mặc định
                false,        // isDescending mặc định
                academicYearId,
                null,         // semesterId
                null,         // gradeLevelsId
                null          // classId
            );
            return StatusCode(response.Code == 0 ? 200 : 400, response);
        }

        [HttpGet("by-grade")]
        public IActionResult GetByGrade(
            [FromQuery] int gradeLevelsId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var response = _service.GetAll(
                page,
                pageSize,
                null,
                "Id",
                false,
                null,         // academicYearId
                null,         // semesterId
                gradeLevelsId,
                null          // classId
            );
            return StatusCode(response.Code == 0 ? 200 : 400, response);
        }

        [HttpGet("by-class")]
        public IActionResult GetByClass(
            [FromQuery] int classId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var response = _service.GetAll(
                page,
                pageSize,
                null,
                "Id",
                false,
                null,         // academicYearId
                null,         // semesterId
                null,         // gradeLevelsId
                classId
            );
            return StatusCode(response.Code == 0 ? 200 : 400, response);
        }
        [HttpGet("{examScheduleId}/details")]
        public IActionResult GetScheduleWithClasses(long examScheduleId)
        {
            var response = _service.GetScheduleWithClasses(examScheduleId);
            return StatusCode(response.Code == 0 ? 200 : 404, response);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var response = _service.GetById(id);
            return StatusCode(response.Code == 0 ? 200 : 404, response);
        }

        [HttpGet("calendar-structured")]
        public IActionResult GetForCalendarStructured(
        [FromQuery] int academicYearId,
        [FromQuery] int? semesterId = null,
        [FromQuery] int? gradeLevelsId = null,
        [FromQuery] int? classId = null)
        {
            var resp = _service.GetForCalendarStructured(academicYearId, semesterId, gradeLevelsId, classId);
            if (resp.Code != 0)
                return BadRequest(resp);
            return Ok(resp);
        }
        [HttpPost]
        public IActionResult Create([FromBody] ExamScheduleRequest request)
        {
            var response = _service.Create(request);
            return StatusCode(response.Code == 0 ? 201 : 400, response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ExamScheduleRequest request)
        {
            var response = _service.Update(id, request);
            return StatusCode(response.Code == 0 ? 200 : 404, response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = _service.Delete(id);
            return StatusCode(response.Code == 0 ? 200 : 404, response);
        }

       
    }
}
