using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResignationController : ControllerBase
    {
        private readonly IResignationService _service;
        public ResignationController(IResignationService service)
        {
            _service = service;
        }

        [HttpGet()]
        public IActionResult GetResignation([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetResignation(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("getResignationnopaging")]
        public IActionResult GetResignationNoPaging()
        {
            var response = _service.GetResignationNoPaging();
            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateResignation([FromBody] ResignationRequest ResignationRequest)
        {
            var response = _service.CreateResignation(ResignationRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }


        [HttpGet("{id}")]
        public IActionResult GetResignationById(long id)
        {
            var response = _service.GetResignationById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("getbyteacherid/{id}")]
        public IActionResult GetResignationByTeacherId(long id)
        {
            var respose = _service.GetResignationByTeacherId(id);
            return respose.Code ==0 ? Ok(respose) : NotFound(respose);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateResignation(long id, [FromBody] ResignationRequest Resignation_UpdateRequest)
        {
            var response = _service.UpdateResignation(id, Resignation_UpdateRequest);

            if (response.Code == 0)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserStatus(long id)
        {
            var response = _service.DeleteResignation(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
