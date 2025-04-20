using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/teaching-assignments")]
    public class TeachingAssignmentsController : ControllerBase
    {
        private readonly ITeachingAssignmentsService _service;

        public TeachingAssignmentsController(ITeachingAssignmentsService service)
        {
            _service = service;
        }

        [HttpGet("class-not-expired")]
        public IActionResult GetTeachingAssignmentsClassStatusTrue(
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 10,
            [FromQuery] string? sortColumn = "Id",
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? searchSubject = null,
            [FromQuery] int? subjectId = null,
            [FromQuery] int? subjectGroupId = null)
        {
            var response = _service.GetTeachingAssignmentsNotExpired(page, pageSize, sortColumn, sortOrder, searchSubject, subjectId, subjectGroupId);
            return Ok(response);
        }

        [HttpGet("class-expired")]
        public IActionResult GetTeachingAssignmentsClassStatusFalse(
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 10,
            [FromQuery] string? sortColumn = "Id",
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? searchSubject = null,
            [FromQuery] int? subjectId = null,
            [FromQuery] int? subjectGroupId = null)
        {
            var response = _service.GetTeachingAssignmentsExpired(page, pageSize, sortColumn, sortOrder, searchSubject, subjectId, subjectGroupId);
            return Ok(response);
        }

        [HttpGet("getTeacherByAcademicYear-SubjectGroup")]
        public IActionResult GetTeacherByAcademicYearAndSubjectGroup( 
            [FromQuery] int? academicYearId = null,
            [FromQuery] int? subjectGroupId = null,
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 10,
            [FromQuery] string? sortColumn = "Id",
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? search = null)
           
        {
            var response = _service.GetTeacherByAcademicYearAndSubjectGroup(academicYearId,subjectGroupId,page, pageSize, sortColumn, sortOrder, search);
            return Ok(response);
        }

        [HttpGet("getByTeacherId")]
        public IActionResult GetTeachingAssignmentsByTeacher(
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 10,
            [FromQuery] string? sortColumn = "Id",
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] int? teacherId = null)

        {
            var response = _service.GetTeachingAssignmentsByTeacher(page, pageSize, sortColumn, sortOrder, teacherId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTeachingAssignmentById(int id)
        {
            var response = _service.GetTeachingAssignmentById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateTeachingAssignment([FromBody] TeachingAssignmentsRequest request)
        {
            var response = _service.CreateTeachingAssignment(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeachingAssignment(int id, [FromBody] TeachingAssignmentsRequest request)
        {
            var response = _service.UpdateTeachingAssignment(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public IActionResult DeleteTeachingAssignment([FromQuery] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest(ApiResponse<bool>.BadRequest("Danh sách ID phân công giảng dạy không được để trống"));
            }

            var response = _service.DeleteTeachingAssignment(ids);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update-status/{id}")]
        public IActionResult UpdateTimeTeachingAssignment(int id)
        {
            var response = _service.UpdateTimeTeachingAssignment(id);
            return Ok(response);
        }


    }
}
