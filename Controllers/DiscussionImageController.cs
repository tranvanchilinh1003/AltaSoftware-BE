using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{

    [ApiController]
    [Route("api/discussion-image")]
    public class DiscussionImageController : ControllerBase
    {
        private readonly IDiscussionImageService _service;

        public DiscussionImageController(IDiscussionImageService service)
        {
            _service = service;
        }

        // Lấy danh sách ảnh theo DiscussionId
        [HttpGet("discussion/{discussionId}")]
        public IActionResult GetDiscussionImagesByDiscussionId(long discussionId)
        {
            var response = _service.GetDiscussionImagesByDiscussionId(discussionId);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        //Lấy ảnh theo Id   
        [HttpGet("{id}")]
        public IActionResult GetDiscussionImageById(long id)
        {
            var response = _service.GetDiscussionImageById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        //Tạo ảnh mới cho Discussion
        [HttpPost]
        public IActionResult CreateDiscussionImage([FromBody] DiscussionImageRequest request)
        {   
            var response = _service.CreateDiscussionImage(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
        // Cập nhật ảnh
        [HttpPut("{id}")]
        public IActionResult UpdateDiscussionImage(long id, [FromBody] DiscussionImageRequest request)
        {
            
            var response = _service.UpdateDiscussionImage(id, request);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        //Xóa ảnh theo ID
        [HttpDelete("{id}")]
        public IActionResult DeleteDiscussionImage(long id)
        {
            var response = _service.DeleteDiscussionImage(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
