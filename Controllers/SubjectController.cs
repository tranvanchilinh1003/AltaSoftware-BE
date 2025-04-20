using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/subjects")]
    public class SubjectController: ControllerBase
    {
        private readonly ISubjectService _service;

        public SubjectController(ISubjectService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetSubject([FromQuery] int? page = null, [FromQuery] int? pageSize = null,
            [FromQuery] string? search = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortOrder = null)
        {
            var response = _service.GetSubject(page, pageSize, search, sortColumn, sortOrder);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
        [HttpGet("get-by-academic-year")]
        public IActionResult GetSubjectByAcademicYear([FromQuery] int? page = null, [FromQuery] int? pageSize = null,
            [FromQuery] string? search = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortOrder = null,
            [FromQuery] int? academicYearId = null)
        {
            var response = _service.GetSubjectByAcademicYear(page, pageSize, search, sortColumn, sortOrder, academicYearId);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
        [HttpGet("get-by-subject-group")]
        public IActionResult GetSubjectBySubjectGroup([FromQuery] int? page = null, [FromQuery] int? pageSize = null,
            [FromQuery] string? search = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortOrder = null,
            [FromQuery] int? subjectGroupId = null)
        {
            var response = _service.GetSubjectBySubjectGroup(page, pageSize, search, sortColumn, sortOrder, subjectGroupId);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetSubjectById(long id)
        {
            var response = _service.GetSubjectById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateSubjectType([FromBody] SubjectRequest request)
        {
            var response = _service.CreateSubject(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSubject(long id, [FromBody] SubjectRequest request)
        {

            var response = _service.UpdateSubject(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubject(long id)
        {
            var response = _service.DeleteSubject(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
