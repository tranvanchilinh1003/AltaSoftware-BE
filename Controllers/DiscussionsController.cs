using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc; 
namespace ISC_ELIB_SERVER.Controllers
{
     
    [ApiController]
    [Route("api/discussions")]
    public class DiscussionsController : ControllerBase
    {
        private readonly IDiscussionsService _service;

        public DiscussionsController(IDiscussionsService service)
        {
            _service = service;
        }

        // Lấy danh sách thảo luận
        [HttpGet]
        public IActionResult GetDiscussions([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetDiscussions(page, pageSize, search, sortColumn, sortOrder);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        // Lấy thảo luận theo ID
        [HttpGet("{id}")]
        public IActionResult GetDiscussionById(long id)
        {
            var response = _service.GetDiscussionById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        // Tạo mới thảo luận
        [HttpPost]
        public IActionResult CreateDiscussion([FromBody] DiscussionRequest discussionRequest)
        {
            var response = _service.CreateDiscussion(discussionRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        // Cập nhật thảo luận
        [HttpPut("{id}")]
        public IActionResult UpdateDiscussion(long id, [FromBody] DiscussionRequest discussionRequest)
        {
            var response = _service.UpdateDiscussion(id, discussionRequest);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        // Xóa thảo luận
        [HttpDelete("{id}")]
        public IActionResult DeleteDiscussion(long id)
        {
            var response = _service.DeleteDiscussion(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
