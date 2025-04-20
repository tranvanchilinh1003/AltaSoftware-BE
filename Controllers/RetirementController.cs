using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/retirement")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class RetirementController : ControllerBase
    {
        private readonly IRetirementService _service;
        public RetirementController(IRetirementService service)
        {
            _service = service;
        }

        [HttpGet()]
        public IActionResult GetRetirement([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetRetirement(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("get-Retirement-no-paging")]
        public IActionResult GetRetirementNoPaging()
        {
            var response = _service.GetRetirementNoPaging();
            return Ok(response);
        }
        [HttpPost]
        public IActionResult CreateRetirement([FromBody] RetirementRequest RetirementRequest)
        {
            var response = _service.CreateRetirement(RetirementRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }


        [HttpGet("{id}")]
        public IActionResult GetRetirementById(long id)
        {
            var response = _service.GetRetirementById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("get-by-teacherid/{id}")]
        public IActionResult GetRetirementByTeacherId(long id)
        {
            var respose = _service.GetRetirementByTeacherId(id);
            return respose.Code == 0 ? Ok(respose) : NotFound(respose);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateRetirement(long id, [FromBody] RetirementRequest Retirement_UpdateRequest)
        {
            var response = _service.UpdateRetirement(id, Retirement_UpdateRequest);

            if (response.Code == 0)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }

        [HttpPut("putByTeacherId/{id}")]
        public IActionResult UpdateRetirementByTeacherId(long id, [FromBody] RetirementRequest Retirement_UpdateRequest)
        {
            var response = _service.UpdateRetirementByTeacherId(id, Retirement_UpdateRequest);

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
        public IActionResult DeleteRetirement(long id)
        {
            var response = _service.DeleteRetirement(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
    
}
