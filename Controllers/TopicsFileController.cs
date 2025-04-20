using Microsoft.AspNetCore.Mvc;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsFileController : ControllerBase
    {
        private readonly ITopicsFileService _topicsFileService;

        public TopicsFileController(ITopicsFileService topicsFileService)
        {
            _topicsFileService = topicsFileService;
        }

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    return Ok(_topicsFileService.GetAll());
        //}

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10, [FromQuery] string? search = "", string sortColumn = "Id", string sortOrder = "asc")
        {
            var result = _topicsFileService.GetAll(page, pageSize, search, sortColumn, sortOrder);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_topicsFileService.GetById(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] TopicsFileRequest request)
        {
            return Ok(_topicsFileService.Create(request));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TopicsFileRequest request)
        {
            return Ok(_topicsFileService.Update(id, request));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_topicsFileService.Delete(id));
        }
    }
}
