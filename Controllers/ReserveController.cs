using Autofac;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/reserves")]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _service;

        public ReserveController(IReserveService service)
        {
            _service = service;
        }

        // GET: api/reserves
        [HttpGet]
        public IActionResult GetActiveReserves([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetActiveReserves(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        // GET: api/reserves/{id}
        [HttpGet("{id}")]
        public IActionResult GetReserveById(long id)
        {
            var response = _service.GetReserveById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        // GET: api/reserves/student/{studentId}
        [HttpGet("student/{studentId}")]
        public IActionResult GetReserveByStudentId(int studentId)
        {
            var response = _service.GetReserveByStudentId(studentId);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        //Post: api/reserves
        [Authorize]
        [HttpPost]
        public IActionResult CreateReserve([FromBody] ReserveRequest reserveRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            reserveRequest.UserId = GetUserId() ?? throw new InvalidOperationException("Không tìm thấy thông tin người dùng");
            reserveRequest.ReserveDate = DateTimeUtils.ConvertToUnspecified(reserveRequest.ReserveDate) ?? throw new InvalidOperationException("ReserveDate không được để trống");
            var response = _service.CreateReserve(reserveRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateReserve(long id, [FromBody] ReserveRequest reserveRequest)
        {
            reserveRequest.UserId = GetUserId() ?? throw new InvalidOperationException("Không tìm thấy thông tin người dùng");
            reserveRequest.ReserveDate = DateTimeUtils.ConvertToUnspecified(reserveRequest.ReserveDate) ?? throw new InvalidOperationException("ReserveDate không được để trống");
            var response = _service.UpdateReserve(id, reserveRequest);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReserve(long id)
        {
            var response = _service.DeleteReserve(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        // Lấy userId từ token JWT
        private int? GetUserId()
        {
            var userIdString = User.FindFirst("Id")?.Value;
            Console.WriteLine($"User.FindFirst(\"Id\"): {userIdString}");

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return null; // Trả về null nếu không tìm thấy hoặc parse thất bại
            }

            return userId;
        }
    }
}
