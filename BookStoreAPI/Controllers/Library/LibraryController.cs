using BookStoreAPI.Services.Library;
using BookStoreViewModels.ViewModels.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController(ILibraryService libraryService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibraryItemsViewModel>>> GetUserLibraryItemsAsync()
        {
            var ebooks = await libraryService.GetAllEbooksAvailableForUserAsync();
            return Ok(ebooks);
        }
    }
}
