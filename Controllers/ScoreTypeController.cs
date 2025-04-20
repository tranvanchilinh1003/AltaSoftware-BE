using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/score-type")]
    public class ScoreTypeController : ControllerBase
    {
        private readonly IScoreTypeService _service;

        public ScoreTypeController(IScoreTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetScoreType([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? search = null,[FromQuery] string? sortColumn = "Id", [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetScoreTypes(page, pageSize,search, sortColumn, sortOrder);
            return Ok(response);
        }


        [HttpGet("{id}")]
        public IActionResult GetScoreTypeById(int id)
        {
            var response = _service.GetScoreTypeById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateScoreType([FromBody] ScoreTypeRequest scoreTypeRequest)
        {
            var response = _service.CreateScoreType(scoreTypeRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateScoreType(int id, [FromBody] ScoreTypeRequest scoreTypeRequest)
        {

            var response = _service.UpdateScoreType(id, scoreTypeRequest);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteScoreType(int id)
        {
            var response = _service.DeleteScoreType(id);

            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

    }
}
