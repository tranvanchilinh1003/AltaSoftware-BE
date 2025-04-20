using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/answer-images")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AnswerImagesQaController : ControllerBase
    {
        private readonly IAnswerImagesQaService _service;

        public AnswerImagesQaController(IAnswerImagesQaService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAnswerImages([FromQuery] long? answerId = null)
        {
            var response = _service.GetAnswerImages(answerId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetAnswerImageById(long id)
        {
            var response = _service.GetAnswerImageById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateAnswerImage([FromBody] AnswerImagesQaRequest request)
        {
            var response = _service.CreateAnswerImage(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAnswerImage(long id, [FromBody] AnswerImagesQaRequest request)
        {
            var response = _service.UpdateAnswerImage(id, request);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnswerImage(long id)
        {
            var response = _service.DeleteAnswerImage(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
