using Autofac.Core;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/training-program")]
    public class TrainingProgramsController : ControllerBase
    {
        private readonly ITrainingProgramService _service;

        public TrainingProgramsController(ITrainingProgramService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTrainingPrograms([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetTrainingPrograms(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("by-teacher/{teacherId}")]
        public IActionResult GetTrainingProgramsByTeacherId(int teacherId, [FromQuery] string? search = "")
        {
            var result = _service.GetTrainingProgramsByTeacherId(teacherId, search);
            return Ok(result);
        }

        [HttpGet("{id}/teacher/{teacherId}")]
        public IActionResult GetTrainingProgramsById(long id, long teacherId)
        {
            var response = _service.GetTrainingProgramsById(id, teacherId);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }


        [HttpPost]
        public IActionResult CreateTrainingProgram([FromBody] TrainingProgramsRequest trainingProgramsRequest)
        {
            var response = _service.CreateTrainingPrograms(trainingProgramsRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTrainingProgram(long id, [FromBody] TrainingProgramsRequest trainingProgramsRequest)
        {
            var response = _service.UpdateTrainingPrograms(id, trainingProgramsRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteTrainingPrograms(long id, long teacherId)
        {
            var response = _service.DeleteTrainingPrograms(id, teacherId);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

    }
}
