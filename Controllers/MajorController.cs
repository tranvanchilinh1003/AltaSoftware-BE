using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/major")]
    public class MajorController : ControllerBase
    {
        private readonly IMajorService _service;

        public MajorController(IMajorService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetMajor([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetMajor(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetMajorById(long id)
        {
            var response = _service.GetMajorById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateMajor([FromBody] MajorRequest majorRequest)
        {
            var response = _service.CreateMajor(majorRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMajor(long id, [FromBody] MajorRequest majorRequest)
        {
            var response = _service.UpdateMajor(id, majorRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMajor(long id)
        {
            var response = _service.DeleteMajor(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

    }
}
