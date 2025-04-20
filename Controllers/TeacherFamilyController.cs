using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/teacherfamilies")]
    public class TeacherFamilyController : ControllerBase
    {
        private readonly ITeacherFamilyService _service;

        public TeacherFamilyController(ITeacherFamilyService service)
        {
            _service = service;
        }

        // Lấy danh sách TeacherFamily
        [HttpGet]
        public IActionResult GetTeacherFamilies()
        {
            var response = _service.GetTeacherFamilies();
            return Ok(response);
        }

        // Lấy thông tin TeacherFamily theo ID
        [HttpGet("{id}")]
        public IActionResult GetTeacherFamilyById(long id)
        {
            var response = _service.GetTeacherFamilyById(id);
            if (response == null)
                return NotFound(new { Code = 1, Message = "Không tìm thấy bản ghi." });

            return Ok(response);
        }

        // Thêm mới TeacherFamily
        [HttpPost]
        public IActionResult CreateTeacherFamily([FromBody] TeacherFamilyRequest request)
        {
            if (request == null)
                return BadRequest(new { Code = 1, Message = "Dữ liệu không hợp lệ." });

            var response = _service.CreateTeacherFamily(request);

            if (response.Code == 1)
                return BadRequest(response);

            return CreatedAtAction(nameof(GetTeacherFamilyById), new { id = response.Data.Id }, response);
        }

        // Cập nhật TeacherFamily
        [HttpPut("{id}")]
        public IActionResult UpdateTeacherFamily(long id, [FromBody] TeacherFamilyRequest request)
        {
            if (request == null)
                return BadRequest(new { Code = 1, Message = "Dữ liệu không hợp lệ." });

            var response = _service.UpdateTeacherFamily(id, request);
            if (response == null)
                return NotFound(new { Code = 1, Message = "Không tìm thấy bản ghi để cập nhật." });

            return Ok(response);
        }

        // Xóa mềm TeacherFamily
        [HttpDelete("{id}")]
        public IActionResult DeleteTeacherFamily(long id)
        {
            var response = _service.DeleteTeacherFamily(id);
            if (response == null)
                return NotFound(new { Code = 1, Message = "Không tìm thấy bản ghi để xóa." });

            return Ok(response);
        }
    }
}
