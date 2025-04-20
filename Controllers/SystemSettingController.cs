using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/system-settings")]
    public class SystemSettingController : ControllerBase
    {
        private readonly ISystemSettingsService _service;

        public SystemSettingController(ISystemSettingsService service)
        {
            _service = service;
        }


        [HttpGet("user")]

        public IActionResult GetSystemSettingByUser()
        {
            var response = _service.GetSystemSettingByUser();
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        public IActionResult CreateOrUpdateSystemSetting([FromBody] SystemSettingRequest systemSettingRequest)
        {
            var response = _service.CreateOrUpdateSystemSetting(systemSettingRequest);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }


        [HttpDelete("{id}")]
        
        public IActionResult DeleteSystemSetting(int id)
        {
            var response = _service.DeleteSystemSetting(id);
            return StatusCode(response.Code, response);
        }
    }
}
