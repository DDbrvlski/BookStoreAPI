using BookStoreAPI.Services.Library;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.User)]
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
