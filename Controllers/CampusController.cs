using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/campuses")]
    [ApiController]
    public class CampusController : ControllerBase
    {
        private readonly ICampusService _service;

        public CampusController(ICampusService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetCampuses(
                [FromQuery] int? page = null,
                [FromQuery] int? pageSize = null,
                [FromQuery] string? sortColumn = null,
                [FromQuery] string? sortOrder = null,
                [FromQuery] string? search = ""
             )
        {
            var response = _service.GetCampuses(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetCampusById(long id)
        {
            var response = _service.GetCampusById(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public IActionResult CreateCampus([FromBody] CampusRequest request)
        {
            var response = _service.CreateCampus(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCampus(long id, [FromBody] CampusRequest request)
        {
            var response = _service.UpdateCampus(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCampus(long id)
        {
            var response = _service.DeleteCampus(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
