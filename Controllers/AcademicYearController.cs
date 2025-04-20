using System.Threading.Tasks;
using ISC_ELIB_SERVER.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/academic-years")]
    public class AcademicYearController : ControllerBase
    {
        private readonly IAcademicYearService _service;

        public AcademicYearController(IAcademicYearService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAcademicYears(
            [FromQuery] int? page = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortOrder = null
            )
        {
            var response = _service.GetAcademicYears(page, pageSize, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetAcademicYearById(long id)
        {
            var response = _service.GetAcademicYearById(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAcademicYear([FromBody] AcademicYearRequest request)
        {
            var response = await _service.CreateAcademicYear(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAcademicYear(long id, [FromBody] ICollection<AcademicYearSemesterRequest> request)
        {
            var response = _service.UpdateAcademicYear(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAcademicYear(long id)
        {
            var response = _service.DeleteAcademicYear(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}