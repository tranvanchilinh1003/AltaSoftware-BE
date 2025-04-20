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
    [Route("api/ChangeClass")]
    public class ChangeClassController : ControllerBase
    {
        private readonly IChangeClassService _service;

        public ChangeClassController(IChangeClassService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetChangeClasses([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetChangeClasses(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetChangeClassById(long id)
        {
            var response = _service.GetChangeClassById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateChangeClass([FromBody] ChangeClass_AddRequest ChangeClassRequest)
        {
            var response = _service.CreateChangeClass(ChangeClassRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateChangeClass(long id, [FromBody] ChangeClass_UpdateRequest ChangeClass)
        {
            var response = _service.UpdateChangeClass(id , ChangeClass);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteChangeClass(long id)
        {
            var response = _service.DeleteChangeClass(id);
            var result = _service.GetChangeClassesNormal();
            return response.Code == 0 ? Ok(result) : BadRequest(result);
        }

    }
}
