using DotNetEnv;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchivementController : ControllerBase
    {
        private readonly IAchivementService _service;
        public AchivementController(IAchivementService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAchivements([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetAchivements(page, pageSize, search, sortColumn, sortOrder);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        //[HttpGet("{id}")]
        //public IActionResult GetAchivementById(int id)
        //{
        //    var response = _service.GetAchivementById(id);
        //    return response.Code == 0 ? Ok(response) : NotFound(response);
        //}

        //[HttpPost]
        //public IActionResult CreateAchivement([FromBody] AchivementRequest achivementRequest)
        //{
        //    var response = _service.CreateAchivement(achivementRequest);
        //    return response.Code == 0 ? Ok(response) : BadRequest(response);
        //}

        [HttpGet("GetAwards")]
        public IActionResult GetAwards([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetAwards(page, pageSize, search, sortColumn, sortOrder);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetDisciplines")]
        public IActionResult GetDisciplines([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetDisciplines(page, pageSize, search, sortColumn, sortOrder);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost("CreateAward")]
        public IActionResult CreateAward([FromBody] AchivementRequest achivementRequest)
        {
            achivementRequest.TypeId = 0;

            var response = _service.CreateAchivement(achivementRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("UpdateAward/{id}")]
        public IActionResult UpdateAward(int id, [FromBody] AchivementRequest achivementRequest)
        {
            achivementRequest.TypeId = 0;
            var response = _service.UpdateAchivement(id, achivementRequest);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost("CreateDiscipline")]
        public IActionResult CreateDiscipline([FromBody] AchivementRequest achivementRequest)
        {
            achivementRequest.TypeId = 1;
            var response = _service.CreateAchivement(achivementRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("UpdateDiscipline/{id}")]
        public IActionResult UpdateDiscipline(int id, [FromBody] AchivementRequest achivementRequest)
        {
            achivementRequest.TypeId = 1;
            var response = _service.UpdateAchivement(id, achivementRequest);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAchivement(int id, [FromBody] AchivementRequest achivementRequest)
        {    
            var response = _service.UpdateAchivement(id, achivementRequest);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAchivement(int id)
        {
            var response = _service.DeleteAchivement(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Vui lòng chọn file hợp lệ.");
            }

            var data = _service.GetAchivementById(id);
            if (data.Code != 0 || data.Data == null)
            {
                return NotFound("Không tìm thấy thành tích.");
            }

            try
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string fileName = Path.GetFileName(file.FileName);

                string filePath = Path.Combine(uploadFolder, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
                else
                {
                    using var stream = new FileStream(filePath, FileMode.CreateNew);
                    await file.CopyToAsync(stream);
                }

                var request = new AchivementRequest
                {
                    TypeId = data.Data.TypeId,
                    UserId = data.Data.Users?.Id ?? null,
                    File = fileName,
                    Content = data.Data.Content ?? "Nội dung trống",
                    DateAwarded = data.Data.DateAwarded ?? DateTime.Now
                };

                var response = _service.UpdateAchivement(id, request);
                return response.Code == 0 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi lưu file: " + ex.Message);
            }
        }
    }
}
