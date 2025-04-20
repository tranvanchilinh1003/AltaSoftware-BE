using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/themes")]
    public class ThemesController: ControllerBase
    {
        private readonly IThemesService _service;

        public ThemesController(IThemesService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetUserStatuses([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetThemes(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetThemesById(long id)
        {
            var response = _service.GetThemesById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateThemes([FromBody] ThemesRequest themesRequest)
        {
            var response = _service.CreateThemes(themesRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateThemes(long id, [FromBody] ThemesRequest themesRequest)
        {
            var response = _service.UpdateThemes(id ,themesRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteThemes(long id)
        {
            var response = _service.DeleteThemes(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

    }
}
