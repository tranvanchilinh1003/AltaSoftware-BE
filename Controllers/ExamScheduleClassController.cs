using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/exam-schedule-classes")]
    [ApiController]
    public class ExamScheduleClassController : ControllerBase
    {
        private readonly IExamScheduleClassService _service;

        public ExamScheduleClassController(IExamScheduleClassService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<ApiResponse<PagedResult<ExamScheduleClassResponse>>> GetAll(
      [FromQuery] int page = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] string? searchTerm = null,
      [FromQuery] string? sortBy = null,
      [FromQuery] string? sortOrder = "asc")
        {
            return Ok(_service.GetAll(page, pageSize, searchTerm, sortBy, sortOrder));
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<ExamScheduleClassResponse>> GetById(long id)
        {
            var response = _service.GetById(id);
            if (response.Code == 1) return NotFound(response);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<ApiResponse<ExamScheduleClassResponse>> Create([FromBody] ExamScheduleClassRequest request)
        {
            var response = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response);
        }

        [HttpPut("{id}")]
        public ActionResult<ApiResponse<ExamScheduleClassResponse>> Update(long id, [FromBody] ExamScheduleClassRequest request)
        {
            var response = _service.Update(id, request);
            if (response.Code == 1) return NotFound(response);
            return Ok(response);
        }
        [HttpPut("{examScheduleId}/class/{classId}/student-count")]
        public IActionResult UpdateStudentCount(int examScheduleId, int classId, [FromBody] int studentCount)
        {
            var result = _service.UpdateStudentCount(examScheduleId, classId, studentCount);
            return Ok(result);
        }

        [HttpDelete("{examScheduleId}/class/{classId}")]
        public IActionResult RemoveClassFromSchedule(int examScheduleId, int classId)
        {
            var result = _service.RemoveClassFromSchedule(examScheduleId, classId);
            return Ok(result);
        }
    }
}
