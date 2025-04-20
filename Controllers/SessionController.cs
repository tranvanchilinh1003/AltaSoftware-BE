using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;
using ISC_ELIB_SERVER.Services.Interfaces;
using Autofac.Core;


namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/sessions")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _service;

        public SessionController(ISessionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lấy danh sách Sessions có phân trang, tìm kiếm và sắp xếp.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "Id",
            [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetSessions(page, pageSize, search, sortColumn, sortOrder);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        /// <summary>
        /// Lấy chi tiết một Session theo ID.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var response = _service.GetSessionById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        /// <summary>
        /// Tạo một Session mới.
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] SessionRequest request)
        {
            var response = _service.CreateSession(request);
            return response.Code == 0 ? Ok(response) : Conflict(response);
        }

        /// <summary>
        /// Cập nhật thông tin Session theo ID.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SessionRequest request)
        {
            var response = _service.UpdateSession(id, request);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        /// <summary>
        /// Xóa một Session theo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = _service.DeleteSession(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost("join")]
        public IActionResult JoinSession([FromBody] JoinSessionRequest request)
        {
            var result = _service.JoinSession(request);
            return result.Code == 0 ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        ///  lấy buổi học theo học viên.
        /// </summary>
        [HttpGet("getSession-by-students")]
        public ActionResult<ICollection<SessionStudentResponse>> GetFilteredSessions(
            [FromQuery] SessionStudentFilterRequest request,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
            )
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return BadRequest(ApiResponse<ICollection<SessionStudentResponse>>.NotFound("No userId found"));
            }
            // request.studentId = int.Parse(userId);
            var sessions = _service.GetFilteredSessions(int.Parse(userId), page, pageSize, request);
            return Ok(sessions);
        }

        /// <summary>   

        [HttpGet("getTeacher-by-SubjectGroup")]
        public ActionResult<ICollection<TeacherDto>> GetTeachersBySubjectGroup()
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return BadRequest(ApiResponse<ICollection<TeacherDto>>.NotFound("No userId found"));
            }
            // request.studentId = int.Parse(userId);
            var teacher = _service.GetTeachersBySubjectGroup(int.Parse(userId));
            return Ok(teacher);

        }


        [HttpGet("getClass-by-teacher")]
        public ActionResult<ICollection<TeacherDto>> GetClassByTeacherID()
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return BadRequest(ApiResponse<ICollection<TeacherDto>>.NotFound("No teacherId found"));
            }
            // request.studentId = int.Parse(userId);
            var teacher = _service.GetClassByTeacher(int.Parse(userId));
            return Ok(teacher);

        }

        [HttpPost("create-session-teacher")]
        public IActionResult Create_teacher([FromBody] SessionRequestTeacher request)
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return BadRequest(ApiResponse<ICollection<TeacherDto>>.NotFound("No teacherId found"));
            }
            var response = _service.CreateSession(int.Parse(userId), request);
            return response.Code == 0 ? Ok(response) : Conflict(response);
        }


    }
}
