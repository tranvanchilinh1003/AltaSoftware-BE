using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/teacherinfos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TeacherInfoController : ControllerBase
    {
        private readonly ITeacherInfoService _teacherInfoService;

        public TeacherInfoController(ITeacherInfoService teacherInfoService)
        {
            _teacherInfoService = teacherInfoService;
        }

        // GET: api/teacherinfos
        [HttpGet]
        public ActionResult<ApiResponse<ICollection<TeacherInfoResponses>>> GetTeacherInfos(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc")
        {
            return Ok(_teacherInfoService.GetTeacherInfos(page, pageSize, search, sortColumn, sortOrder));
        }

        // GET: api/teacherinfos/{id}
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<TeacherInfoResponses>> GetTeacherInfoById(int id)
        {
            return Ok(_teacherInfoService.GetTeacherInfoById(id));
        }

        // GET: api/teacherinfos/code/{code}
        [HttpGet("code/{code}")]
        public ActionResult<ApiResponse<TeacherInfoResponses>> GetTeacherInfoByCode(string code)
        {
            return Ok(_teacherInfoService.GetTeacherInfoByCode(code));
        }

        // POST: api/teacherinfos
        [HttpPost]
        public ActionResult<ApiResponse<TeacherInfoResponses>> CreateTeacherInfo([FromBody] TeacherInfoRequest teacherInfoRequest)
        {
            if (teacherInfoRequest == null)
                return BadRequest("Dữ liệu thông tin giáo viên không hợp lệ.");

            // Nếu cần, chuyển đổi các trường DateTime về kiểu Unspecified trước khi lưu
            teacherInfoRequest.IssuedDate = DateTimeUtils.ConvertToUnspecified(teacherInfoRequest.IssuedDate);
            teacherInfoRequest.UnionDate = DateTimeUtils.ConvertToUnspecified(teacherInfoRequest.UnionDate);
            teacherInfoRequest.PartyDate = DateTimeUtils.ConvertToUnspecified(teacherInfoRequest.PartyDate);

            return Ok(_teacherInfoService.CreateTeacherInfo(teacherInfoRequest));
        }

        // PUT: api/teacherinfos/{id}
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<TeacherInfoResponses>> UpdateTeacherInfo(int id, [FromBody] TeacherInfoRequest teacherInfoRequest)
        {
            if (teacherInfoRequest == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            // Chuyển đổi các trường DateTime nếu cần
            teacherInfoRequest.IssuedDate = DateTimeUtils.ConvertToUnspecified(teacherInfoRequest.IssuedDate);
            teacherInfoRequest.UnionDate = DateTimeUtils.ConvertToUnspecified(teacherInfoRequest.UnionDate);
            teacherInfoRequest.PartyDate = DateTimeUtils.ConvertToUnspecified(teacherInfoRequest.PartyDate);

            // Truyền ID vào service để cập nhật đúng giáo viên
            return Ok(_teacherInfoService.UpdateTeacherInfo(id, teacherInfoRequest));
        }

        // DELETE: api/teacherinfos/{id}
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<TeacherInfoResponses>> DeleteTeacherInfo(int id)
        {
            return Ok(_teacherInfoService.DeleteTeacherInfo(id));
        }
    }
}
