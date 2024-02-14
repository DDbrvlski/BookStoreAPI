using BookStoreAPI.Services.BookItems;
using BookStoreViewModels.ViewModels.Products.BookItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.BookItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookItemsController(IBookItemService bookItemService) : ControllerBase
    {
        [HttpGet]
        [Route("Carousel/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookItemCarouselViewModel>>> GetBookItemsByFormIdForCarouselAsync(int id)
        {
            var bookItems = await bookItemService.GetBookItemsByFormIdForCarouselAsync(id);
            return Ok(bookItems);
        }

        [HttpGet]
        [Route("Store")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookItemViewModel>>> GetBookItemsAsync([FromQuery]BookItemFiltersViewModel bookItemFilters)
        {
            var bookItems = await bookItemService.GetBookItemsAsync(bookItemFilters);
            return Ok(bookItems);
        }

        [HttpGet]
        [Route("Store/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BookItemDetailsViewModel>> GetBookItemDetailsAsync(int id)
        {
            var bookItem = await bookItemService.GetBookItemDetailsAsync(id);
            return Ok(bookItem);
        }

        [HttpGet]
        [Authorize("BookItemsRead")]
        public async Task<ActionResult<IEnumerable<BookItemCMSViewModel>>> GetBookItemsForCMSAync()
        {
            var bookItems = await bookItemService.GetBookItemsForCMSAync();
            return Ok(bookItems);
        }

        [HttpGet("{id}")]
        [Authorize("BookItemsRead")]
        public async Task<ActionResult<BookItemDetailsCMSViewModel>> GetBookItemByIdForCMSAsync(int id)
        {
            var bookItem = await bookItemService.GetBookItemByIdForCMSAsync(id);
            return Ok(bookItem);
        }

        [HttpPost]
        [Authorize("BookItemsWrite")]
        public async Task<IActionResult> PostBookItemAsync(BookItemPostCMSViewModel bookItemModel)
        {
            await bookItemService.CreateBookItemAsync(bookItemModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize("BookItemsEdit")]
        public async Task<IActionResult> PutBookItemAsync(int id, BookItemPostCMSViewModel bookItemModel)
        {
            await bookItemService.UpdateBookItemAsync(id, bookItemModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize("BookItemsDelete")]
        public async Task<IActionResult> DeactivateBookItemAsync(int id)
        {
            await bookItemService.DeactivateBookItemAsync(id);
            return NoContent();
        }
    }
}
