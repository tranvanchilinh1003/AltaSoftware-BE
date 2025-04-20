using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/semesters")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _service;

        public SemesterController(ISemesterService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetSemesters([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? sortColumn = "Id", [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetSemesters(page, pageSize, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("ranking")]
        [Authorize]
        public IActionResult GetScoreSemesters([FromQuery] int? academicYearId = 1, [FromQuery] int? userId = 1)
        {
            //var userId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

            var response = _service.GetScoreBySemesters(userId.Value, academicYearId.Value);
            return Ok(response);
        }

        [HttpGet("student-score")]
        [Authorize]
        public IActionResult GetStudentScoreSemesters([FromQuery] int? academicYearId = 1, [FromQuery] int? userId = 1)
        {
            //var userId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

            var response = _service.GetStudentScores(userId.Value, academicYearId.Value);
            return Ok(response);
        }

        [HttpGet("course")]
        [Authorize]
        public IActionResult GetCourseOfSemesters([FromQuery] int? page = 1,
                                          [FromQuery] int? pageSize = 10,
                                          [FromQuery] string? sortColumn = "Id",
                                          [FromQuery] string? sortOrder = "asc")
        {
            var userId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

            var response = _service.GetCourseOfSemesters(page, pageSize, sortColumn, sortOrder, userId);
            return Ok(response);
        }


        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetSemesterrById(long id)
        {
            var response = _service.GetSemesterById(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateSemester([FromBody] SemesterRequest request)
        {
            var response = _service.CreateSemester(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateSemester(long id, [FromBody] SemesterRequest request)
        {
            var response = _service.UpdateSemester(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteSemester(long id)
        {
            var response = _service.DeleteSemester(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}