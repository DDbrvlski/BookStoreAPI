using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Books;
using BookStoreDto.Dtos.Products.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.Books
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(IBookService bookService) : ControllerBase
    {
        [HttpDelete("{id}")]
        [Authorize("BookDelete")]
        public async Task<IActionResult> DeleteBookAsync(int id)
        {
            await bookService.DeactivateBookAsync(id);
            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BooksCMSDto>>> GetAllBooksAsync()
        {
            return Ok(await bookService.GetAllBooksForCMSAsync());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BookDetailsCMSDto>> GetBookByIdAsync(int id)
        {
            return Ok(await bookService.GetBookDetailsForCMSByIdAsync(id));
        }

        [HttpPost]
        [Authorize("BookWrite")]
        public async Task<IActionResult> PostBookAsync(BookPostDto bookPost)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await bookService.CreateBookAsync(bookPost);

            return Created();
        }

        [HttpPut("{id}")]
        [Authorize("BookEdit")]
        public async Task<IActionResult> PutBookAsync(int id, BookPostDto bookPut)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await bookService.UpdateBookAsync(id, bookPut);

            return Ok();
        }
    }
}
