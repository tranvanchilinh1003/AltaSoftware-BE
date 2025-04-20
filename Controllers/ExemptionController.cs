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
    [Route("api/Exemption")]
    public class ExemptionController : ControllerBase
    {
        private readonly IExemptionService _service;

        public ExemptionController(IExemptionService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetExemptions([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetExemptions(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetExemptionById(long id)
        {
            var response = _service.GetExemptionById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateExemption([FromBody] Exemption_AddRequest ExemptionRequest)
        {
            var response = _service.CreateExemption(ExemptionRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateExemption(long id, [FromBody] Exemption_UpdateRequest Exemption)
        {
            var response = _service.UpdateExemption(id , Exemption);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteExemption(long id)
        {
            var response = _service.DeleteExemption(id);
            var result = _service.GetExemptionsNormal();
            return response.Code == 0 ? Ok(result) : BadRequest(result);
        }

    }
}
