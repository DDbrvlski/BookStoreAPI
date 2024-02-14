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
        [Authorize("BooksDelete")]
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
        [Authorize("BooksWrite")]
        public async Task<IActionResult> PostBookAsync(BookPostDto bookPost)
        {
            if (bookPost == null)
            {
                throw new BadRequestException("Nie można wykonać operacji dodawania, ponieważ przesłany obiekt jest pusty.");
            }
            await bookService.CreateBookAsync(bookPost);

            return Created();
        }

        [HttpPut("{id}")]
        [Authorize("BooksEdit")]
        public async Task<IActionResult> PutBookAsync(int id, BookPostDto bookPut)
        {
            if (bookPut == null)
            {
                throw new BadRequestException("Nie można wykonać operacji aktualizacji, ponieważ przesłany obiekt jest pusty.");
            }
            await bookService.UpdateBookAsync(id, bookPut);

            return Ok();
        }
    }
}
