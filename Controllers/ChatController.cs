using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    
    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _service;

        public ChatController(IChatService service)
        {
            _service = service;
        }

        // Lấy danh sách tin nhắn theo SessionId
        [HttpGet("session/{sessionId}")]
        public IActionResult GetChatsBySessionId(long sessionId)
        {
            var response = _service.GetChatsBySessionId(sessionId);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        //Lấy tin nhắn theo Id
        [HttpGet("{id}")]
        public IActionResult GetChatById(long id)
        {
            var response = _service.GetChatById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        //Tạo tin nhắn mới
        [HttpPost]
        public IActionResult CreateChat([FromBody] ChatRequest chatRequest)
        {
             
            var response = _service.CreateChat(chatRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        //sửa tin nhắn
        [HttpPut("{id}")]
        public IActionResult UpdateChat(long id, [FromBody] ChatUpdateRequest chatRequest)
        {
             
            var response = _service.UpdateChat(id, chatRequest);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
        //Xóa tin nhắn
        [HttpDelete("{id}")]
        public IActionResult DeleteChat(long id)
        {
            var response = _service.DeleteChat(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
 
