using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetNotifications([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetNotifications(page, pageSize, search, sortColumn, sortOrder);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetNotificationById(long id)
        {
            var response = _service.GetNotificationById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateNotification([FromBody] NotificationRequest notificationRequest)
        {
            var senderIdClaim = User.FindFirst("Id")?.Value;

            if (!int.TryParse(senderIdClaim, out int senderId))
            {
                return Unauthorized(ApiResponse<string>.Fail("ID trong token không hợp lệ"));
            }


            var response = _service.CreateNotification(notificationRequest, senderId);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateNotification(long id, [FromBody] NotificationRequest notificationRequest)
        {
            var senderIdClaim = User.FindFirst("Id")?.Value;

            if (!int.TryParse(senderIdClaim, out int senderId))
            {
                return Unauthorized(ApiResponse<string>.Fail("ID trong token không hợp lệ"));
            }

            var response = _service.UpdateNotification(id, notificationRequest, senderId);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(long id)
        {
            var response = _service.DeleteNotification(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
