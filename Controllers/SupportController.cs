using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/support")]
    public class SupportController : ControllerBase
    {
        private readonly ISupportService _service;

        public SupportController(ISupportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetSupport([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? sortColumn = "Id", [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetSupports(page,pageSize,sortColumn,sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetSupportById(int id)
        {
            var response = _service.GetSupportById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateSupport([FromBody] SupportRequest SupportRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = _service.CreateSupport(SupportRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSupport(int id, [FromBody] SupportRequest supportRequest)
        {
            var response = _service.UpdateSupport(id, supportRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSupport(int id)
        {
            var response = _service.DeleteSupport(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

    }
}
