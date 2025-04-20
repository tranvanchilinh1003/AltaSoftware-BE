using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/education-levels")]
    [ApiController]
    public class EducationLevelController : ControllerBase
    {
        private readonly IEducationLevelService _service;

        public EducationLevelController(IEducationLevelService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetEducationLevels([FromQuery] int? page = 1, 
                                                [FromQuery] int? pageSize = 10, 
                                                [FromQuery] string? sortColumn = "Id", 
                                                [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetEducationLevels(page, pageSize, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetEducationLevelById(long id)
        {
            var response = _service.GetEducationLevelById(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateEducationLevel([FromBody] EducationLevelRequest request)
        {
            var response = _service.CreateEducationLevel(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateEducationLevel(long id, [FromBody] EducationLevelRequest request)
        {
            var response = _service.UpdateEducationLevel(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteEducationLevel(long id)
        {
            var response = _service.DeleteEducationLevel(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}