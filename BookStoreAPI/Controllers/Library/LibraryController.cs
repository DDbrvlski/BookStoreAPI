using BookStoreAPI.Services.Library;
using BookStoreData.Models.Accounts;
using BookStoreDto.Dtos.Library;
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
        public async Task<ActionResult<IEnumerable<LibraryItemsDto>>> GetUserLibraryItemsAsync([FromQuery] int libraryStatusId = 0)
        {
            var ebooks = await libraryService.GetAllEbooksAvailableForUserAsync(libraryStatusId);
            return Ok(ebooks);
        }

        [HttpGet("download/{bookItemId}")]
        public async Task<IActionResult> DownloadEbookPdfFileAsync(int bookItemId)
        {
            var file = await libraryService.GetEbookPdfFileAsync(bookItemId);
            return File(file, "application/pdf", "ebook.pdf");
        }
    }
}
