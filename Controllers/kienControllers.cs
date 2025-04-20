using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileUploadAPI.Controllers
{
    [Route("api/kien")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public FileUploadController()
        {
            // Tạo thư mục Uploads nếu chưa tồn tại
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
        try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("File không hợp lệ");

                string filePath = Path.Combine(_uploadFolder, file.FileName);

                // Lưu file vào thư mục Uploads
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { message = "Upload thành công", fileName = file.FileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lưu file", error = ex.Message });
            }
        }
    }
}
