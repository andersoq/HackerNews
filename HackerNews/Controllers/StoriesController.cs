using HackerNews.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IHackerNewsService _service;

        public StoriesController(IHackerNewsService service)
        {
            _service = service;
        }

        [HttpGet("best")]
        public async Task<IActionResult> GetBestStories([FromQuery] int n)
        {
            if (n <= 0)
                return BadRequest("n must be greater than zero");

            var stories = await _service.GetBestStoriesAsync(n);

            return Ok(stories);
        }
    }
}
