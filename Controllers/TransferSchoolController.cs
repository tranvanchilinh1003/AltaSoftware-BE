using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Models;
using Autofac.Core;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.EntityFrameworkCore;

[Route("api/transfer-school")]
[ApiController]
public class TransferSchoolController : ControllerBase
{
    private readonly TransferSchoolRepo _transferSchoolRepo;
    private readonly ITransferSchoolService _service;
    private readonly isc_dbContext _context;


    public TransferSchoolController(TransferSchoolRepo transferSchoolRepo, isc_dbContext context,
        ITransferSchoolService service)
    {
        _transferSchoolRepo = transferSchoolRepo;
        _service = service;
        _context = context;
    }

    /// <summary>
    /// 1️⃣ Lấy danh sách học sinh đã chuyển trường
    /// </summary>
   
    // GET: api/transferschool
    [HttpGet]
    public IActionResult GetTransferSchoolList(
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10,
     [FromQuery] string? search = "",
     [FromQuery] string sortColumn = "id",
     [FromQuery] string sortOrder = "asc")
    {
        // Lấy danh sách chuyển trường (có thể có điều kiện tìm kiếm nếu cần)
        var result = _transferSchoolRepo.GetTransferSchoolList(search);

        // Kiểm tra nếu không có dữ liệu
        if (result == null || !result.Any())
        {
            return NotFound(new { code = 1, message = "Không có dữ liệu chuyển trường" });
        }

        // Tính toán số lượng bản ghi cần phân trang
        var totalItems = result.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Áp dụng phân trang
        var pagedResult = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        // Trả về kết quả phân trang
        return Ok(new
        {
            code = 0,
            message = "Lấy danh sách học sinh chuyển trường thành công",
            data = pagedResult,
            totalItems,
            totalPages,
            page,
            pageSize
        });
    }


    /// <summary>
    /// 2️⃣ Lấy thông tin chuyển trường của một học sinh theo ID
    [HttpGet("byStudentCode/{studentCode}")]
    public async Task<IActionResult> GetTransferSchoolByStudentCode(string studentCode)
    {
        var studentResponse = _service.GetTransferSchoolByStudentCode(studentCode);

        if (studentResponse == null || studentResponse.Data == null)
        {
            return NotFound(new { code = 1, message = "Không tìm thấy học sinh với mã StudentCode đã cung cấp." });
        }

        // Trả về thông tin chuyển trường nếu tìm thấy
        return Ok(new { code = 0, message = "Success", data = studentResponse.Data });
    }

    [HttpGet("byStudentId/{studentId}")]
    public async Task<IActionResult> GetTransferSchoolByStudentId(int studentId)
    {
        // Gọi phương thức bất đồng bộ để lấy thông tin chuyển trường
        var result = _transferSchoolRepo.GetTransferSchoolByStudentId(studentId);

        if (result == null)
        {
            // Trả về NotFound nếu không tìm thấy thông tin
            return NotFound(new { code = 1, message = "Không tìm thấy thông tin chuyển trường cho học sinh này" });
        }

        // Trả về thông tin chuyển trường nếu tìm thấy
        return Ok(new { code = 0, message = "Lấy thông tin chi tiết chuyển trường thành công", data = result });
    }

    // Thêm mới TransferSchool
    [HttpPost]
    public IActionResult CreateTransferSchool([FromBody] TransferSchoolRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.StudentCode))
        {
            return BadRequest(new { Message = "Dữ liệu không hợp lệ. Vui lòng nhập StudentCode." });
        }

        // Lấy userId từ token
        var userId = GetUserId();  // Phương thức này lấy userId từ token
        if (userId == null)
        {
            return BadRequest(new { Message = "Không thể xác định userId." });
        }

        // Gán userId vào request trước khi gọi service
        request.UserId = userId.Value;

        // Gọi service để xử lý logic
        var response = _service.CreateTransferSchool(request);

        if (response.Data == null)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    [HttpPut("{studentCode}")]
    public IActionResult UpdateTransferSchool(string studentCode, [FromBody] TransferSchoolRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.StudentCode))
            return BadRequest(new { Message = "Dữ liệu không hợp lệ. Vui lòng nhập StudentCode." });

        // Lấy userId từ token
        var userId = GetUserId();
        if (userId == null)
        {
            return BadRequest(new { Message = "Không thể xác định userId." });
        }

        // Gán userId vào request trước khi gọi service
        request.UserId = userId.Value;

        // Gọi service để xử lý logic
        var response = _service.UpdateTransferSchool(studentCode,request);

        if (response.Data == null)
            return BadRequest(response);

        return Ok(response);
    }

    // Xóa thông tin chuyển trường theo studentId
    [HttpDelete("byStudentId/{studentId}")]
    public IActionResult DeleteTransferSchoolByStudentId(int studentId)
    {
        // Gọi service để xóa thông tin chuyển trường
        var transferSchool = _service.DeleteTransferSchool(studentId);

        // Nếu không tìm thấy bản ghi
        if (transferSchool == null)
        {
            return NotFound(new { code = 1, message = "Mã học sinh không tồn tại hoặc không có thông tin chuyển trường." });
        }

        // Nếu xóa thành công, trả về thông báo thành công
        return Ok(new { code = 0, message = "Xóa thông tin chuyển trường thành công", data = transferSchool });
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
