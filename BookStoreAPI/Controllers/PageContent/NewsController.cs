using BookStoreAPI.Services.PageElements;
using BookStoreData.Models.Accounts;
using BookStoreData.Models.PageContent;
using BookStoreViewModels.ViewModels.PageContent.News;
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
        public async Task<ActionResult<IEnumerable<NewsViewModel>>> GetAllNewsAsync()
        {
            var news = await newsService.GetAllNewsAsync();
            return Ok(news);
        }

        [HttpGet]
        [Route("Elements")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NewsViewModel>>> GetNumberOfNewsAsync([FromQuery] int numberOfElements = 1)
        {
            var news = await newsService.GetNumberOfNewsAsync(numberOfElements);
            return Ok(news);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<NewsDetailsViewModel>> GetNewsByIdAsync(int id)
        {
            var news = await newsService.GetNewsByIdAsync(id);
            return Ok(news);
        }

        [HttpPost]
        [Authorize("NewsWrite")]
        public async Task<IActionResult> CreateNewsAsync(NewsPostCMSViewModel newsModel)
        {
            await newsService.CreateNewsAsync(newsModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize("NewsEdit")]
        public async Task<IActionResult> EditNewsAsync(int id, NewsPostCMSViewModel newsModel)
        {
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
