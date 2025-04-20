using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/schools")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _service;

        public SchoolController(ISchoolService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchools(
            [FromQuery] int? page = null,
                [FromQuery] int? pageSize = null,
                [FromQuery] string? sortColumn = null,
                [FromQuery] string? sortOrder = null,
                [FromQuery] string? search = ""
        )
        {
            var response = await _service.GetSchools(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolById(long id)
        {
            var response = await _service.GetSchoolById(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public IActionResult CreateSchool([FromBody] SchoolRequest request)
        {
            var response = _service.CreateSchool(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSchool(long id, [FromBody] SchoolRequest request)
        {
            var response = _service.UpdateSchool(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSchool(long id)
        {
            var response = _service.DeleteSchool(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
