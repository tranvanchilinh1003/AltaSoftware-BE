using ISC_ELIB_SERVER.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Reflection.Emit;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/grade-levels")]
    [ApiController]
    public class GradeLevelController : ControllerBase
    {
        private readonly IGradeLevelService _service;

        public GradeLevelController(IGradeLevelService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetGradeLevels(
                [FromQuery] int? page = null,
                [FromQuery] int? pageSize = null,
                [FromQuery] string? sortColumn = null,
                [FromQuery] string? sortOrder = null
             )
        {
            var response = _service.GetGradeLevels(page, pageSize, sortColumn, sortOrder);
            return Ok(response);
        }


        [HttpGet("search")]
        public IActionResult GetGradeLevelsByAyAndSc(
                [FromQuery] string schoolName,
                [FromQuery] int? startYear,
                [FromQuery] int? endYear,
                [FromQuery] int? page = 1,
                [FromQuery] int? pageSize = 10,
                [FromQuery] string? sortColumn = "Id",
                [FromQuery] string? sortOrder = "asc"
            )
        {
            var response = _service.GetGradeLevelsByAyAndSc(page, pageSize, sortColumn, sortOrder, schoolName, startYear, endYear);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetGradeLevelById(long id)
        {
            var response = _service.GetGradeLevelById(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("class/{id}")]
        public IActionResult GetClassOfGradeLevel(long id)
        {
            var response = _service.GetClassOfGradeLevel(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public IActionResult CreateGradeLevel([FromBody] GradeLevelRequest request)
        {
            var response = _service.CreateGradeLevel(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGradeLevel(long id, [FromBody] GradeLevelRequest request)
        {
            var response = _service.UpdateGradeLevel(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGradeLevel(long id)
        {
            var response = _service.DeleteGradeLevel(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}