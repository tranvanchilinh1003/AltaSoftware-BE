using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/class-type")]
    public class ClassTypeController : ControllerBase
    {
        private readonly IClassTypeService _service;

        public ClassTypeController(IClassTypeService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateClassType([FromBody] ClassTypeRequest classTypeRequest)
        {
            var response = _service.CreateClassType(classTypeRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClassType(int id, [FromBody] ClassTypeRequest classTypeRequest)
        {

            var response = _service.UpdateClassType(id, classTypeRequest);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClassType(int id)
        {
            var response = _service.DeleteClassType(id);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetClassTypeById(int id)
        {
            var response = _service.GetClassTypeById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
        [HttpGet]
        public IActionResult GetClassTypesBySchoolYear(
            [FromQuery] int? searchYear = null,
            [FromQuery] string? searchName = null, 
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 10,
            [FromQuery] string? sortColumn = "Id",
            [FromQuery] string? sortOrder = "asc")
        {

            var response = _service.GetClassTypes(page, pageSize, searchYear, searchName, sortColumn, sortOrder);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }



    }
}
