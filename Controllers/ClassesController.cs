using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{

    [ApiController]
    [Route("api/class")]
    public class ClassesController : ControllerBase
    {
        private readonly IClassesService _service;

        public ClassesController(IClassesService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult GetClass([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? search = null, [FromQuery] string? sortColumn = "Id", [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetClass(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("by-grade-academic")]
        public IActionResult GetClassByGradeLevelId([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] int? gradeLevelId = null, [FromQuery] int? academicYearId = null,[FromQuery] string? sortColumn = "Id", [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetClassByGradeLevelIdAndAcademicYearId(page, pageSize, gradeLevelId, academicYearId, sortColumn, sortOrder);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> CreateClass([FromBody] ClassesRequest classesRequest)
        {
            if (classesRequest == null)
            {
                return BadRequest(new { Code = 1, Message = "Dữ liệu không hợp lệ" });
            }

            var response = await _service.CreateClassAsync(classesRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass([FromRoute] int id, [FromBody] ClassesRequest classesRequest)
        {
            if (classesRequest == null)
            {
                return BadRequest(new { Code = 1, Message = "Dữ liệu không hợp lệ" });
            }

            var response = await _service.UpdateClassAsync(id, classesRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }



        [HttpDelete]
        public IActionResult DeleteClass([FromQuery] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest(ApiResponse<bool>.BadRequest("Danh sách ID lớp học không được để trống"));
            }

            var response = _service.DeleteClass(ids);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetClassById(int id)
        {
            var response = _service.GetClassById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportClasses([FromForm] IFormFile file)
        {
            var response = await _service.ImportClassesAsync(file);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPut("class-user-status/{classId}/{userId}")]
        public async Task<IActionResult> UpdateClassUserStatus([FromRoute] int classId, [FromRoute] int userId, [FromBody] UpdateClassUserStatusRequest request)
        {
            var response = await _service.UpdateClassUserStatus(classId, userId, request.NewStatusId);

            return Ok(response);
        }

        [HttpGet("by-subjectId")]
        public IActionResult GetClassBySubjectId([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] int? subjectId = null, [FromQuery] string? sortColumn = "Id", [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetClassBySubjectId(page, pageSize, subjectId, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("classType-co-ban")]
        public IActionResult GetClassByCoBan([FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? sortColumn, [FromQuery] string? sortOrder)
        {
            var response = _service.GetClassByCoBan(page, pageSize, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("classType-nang-cao")]
        public IActionResult GetClassByNangCao([FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? sortColumn, [FromQuery] string? sortOrder)
        {
            var response = _service.GetClassByNangCao(page, pageSize, sortColumn, sortOrder);
            return Ok(response);
        }
    }
}
