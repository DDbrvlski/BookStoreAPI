using BookStoreAPI.Services.Library;
using BookStoreViewModels.ViewModels.Library;
using BookStoreViewModels.ViewModels.Products.BookItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController(ILibraryService libraryService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibraryItemsViewModel>>> GetUserLibraryItemsAsync([FromQuery] int libraryStatusId = 0)
        {
            var ebooks = await libraryService.GetAllEbooksAvailableForUserAsync(libraryStatusId);
            return Ok(ebooks);
        }
    }
}
