using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/teacherlists")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TeacherListController : ControllerBase
    {
        private readonly ITeacherListService _teacherListService;

        public TeacherListController(ITeacherListService teacherListService)
        {
            _teacherListService = teacherListService;
        }

        [HttpGet]
        public ActionResult<ApiResponse<ICollection<TeacherListResponse>>> GetTeacherLists(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "Id",
            [FromQuery] string sortOrder = "asc")
        {
            var result = _teacherListService.GetTeacherLists(page, pageSize, search, sortColumn, sortOrder);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<TeacherListResponse>> GetTeacherListById(int id)
        {
            var result = _teacherListService.GetTeacherListById(id);
            return Ok(result);
        }
    }
}
