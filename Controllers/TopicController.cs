using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/topics")]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _service;

        public TopicController(ITopicService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTopics([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetTopics(page, pageSize, search, sortColumn, sortOrder);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTopicById(int id)
        {
            var response = _service.GetTopicById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateTopic([FromBody] TopicRequest request)
        {
            var response = _service.CreateTopic(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTopic(int id, [FromBody] TopicRequest request)
        {
            var response = _service.UpdateTopic(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTopic(int id)
        {
            var response = _service.DeleteTopic(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
