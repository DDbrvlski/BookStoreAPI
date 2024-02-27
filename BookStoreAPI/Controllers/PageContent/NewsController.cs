using BookStoreAPI.Services.PageElements;
using BookStoreDto.Dtos.PageContent.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController(INewsService newsService) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NewsDto>>> GetAllNewsAsync()
        {
            var news = await newsService.GetAllNewsAsync();
            return Ok(news);
        }

        [HttpGet]
        [Route("Elements")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NewsDto>>> GetNumberOfNewsAsync([FromQuery] int numberOfElements = 1)
        {
            var news = await newsService.GetNumberOfNewsAsync(numberOfElements);
            return Ok(news);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<NewsDetailsDto>> GetNewsByIdAsync(int id)
        {
            var news = await newsService.GetNewsByIdAsync(id);
            return Ok(news);
        }

        [HttpPost]
        [Authorize("NewsWrite")]
        public async Task<IActionResult> CreateNewsAsync(NewsPostCMSDto newsModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await newsService.CreateNewsAsync(newsModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize("NewsEdit")]
        public async Task<IActionResult> EditNewsAsync(int id, NewsPostCMSDto newsModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await newsService.EditNewsAsync(id, newsModel);
            return NoContent();
        }

        [HttpDelete]
        [Authorize("NewsDelete")]
        public async Task<IActionResult> DeactivateNewsAsync(int id)
        {
            await newsService.DeactivateNewsAsync(id);
            return NoContent();
        }
    }
}
