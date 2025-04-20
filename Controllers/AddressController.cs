using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController : ControllerBase
    {
        private readonly GhnService _service;

        public AddressController(GhnService service)
        {
            _service = service;
        }

        [HttpGet("provinces")]
        public async Task<ApiResponse<Province[]>> GetProvices()
        {
            return await _service.GetProvinces();
        }

        [HttpGet("districts")]
        public async Task<ApiResponse<District[]>> GetDistricts([FromQuery] int provinceId)
        {
            return await _service.GetDistricts(provinceId);
        }

        [HttpGet("wards")]
        public async Task<ApiResponse<Ward[]>> GetWards([FromQuery] int districtId)
        {
            return await _service.GetWards(districtId);
        }

    }
}