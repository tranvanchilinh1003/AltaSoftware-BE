using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Sprache;


namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/temporaryleave")]
    public class TemporaryLeaveController : ControllerBase
    {
        private readonly ITemporaryLeaveService _service;

        public TemporaryLeaveController(ITemporaryLeaveService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTemporaryLeaves([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetTemporaryLeaves(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTemporaryLeaveById(long id)
        {
            var response = _service.GetTemporaryLeaveById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateTemporaryLeave([FromBody] TemporaryLeave_AddRequest TemporaryLeaveRequest)
        {
            var response = _service.CreateTemporaryLeave(TemporaryLeaveRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTemporaryLeave(long id, [FromBody] TemporaryLeave_UpdateRequest TemporaryLeave)
        {
            var response = _service.UpdateTemporaryLeave(id , TemporaryLeave);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTemporaryLeave(long id)
        {
            var response = _service.DeleteTemporaryLeave(id);
            var result = _service.GetTemporaryLeavesNormal();
            return response.Code == 0 ? Ok(result) : BadRequest(result);
        }

    }
}
