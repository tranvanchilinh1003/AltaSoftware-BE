using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/subject-groups")]
    public class SubjectGroupController: ControllerBase
    {
        private readonly ISubjectGroupService _service;

        public SubjectGroupController(ISubjectGroupService service) { 
            _service = service;
        }

        [HttpGet]
        public IActionResult GetSubjectGroup([FromQuery] int? page = null, [FromQuery] int? pageSize = null,
            [FromQuery] string? search = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortOrder = null)
        {
            var response = _service.GetSubjectGroup(page, pageSize, search, sortColumn, sortOrder);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetSubjectGroupById(long id)
        {
            var response = _service.GetSubjectGroupById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateSubjectType([FromBody] SubjectGroupRequest request)
        {
            var response = _service.CreateSubjectGroup(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSubjectGroup(long id, [FromBody] SubjectGroupRequest request)
        {

            var response = _service.UpdateSubjectGroup(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubjectGroup(long id)
        {
            var response = _service.DeleteSubjectGroup(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
