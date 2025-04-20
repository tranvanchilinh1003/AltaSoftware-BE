using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/question-images")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class QuestionImagesQaController : ControllerBase
    {
        private readonly IQuestionImagesQaService _service;

        public QuestionImagesQaController(IQuestionImagesQaService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetQuestionImages([FromQuery] long? questionId = null)
        {
            var response = _service.GetQuestionImages(questionId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestionImageById(long id)
        {
            var response = _service.GetQuestionImageById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateQuestionImage([FromBody] QuestionImagesQaRequest request)
        {
            var response = _service.CreateQuestionImage(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateQuestionImage(long id, [FromBody] QuestionImagesQaRequest request)
        {
            var response = _service.UpdateQuestionImage(id, request);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQuestionImage(long id)
        {
            var response = _service.DeleteQuestionImage(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
