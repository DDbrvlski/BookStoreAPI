using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Books;
using BookStoreViewModels.ViewModels.Products.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.Books
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(IBookService bookService) : ControllerBase
    {
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookAsync(int id)
        {
            await bookService.DeactivateBookAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Authorize("BooksRead")]
        public async Task<ActionResult<IEnumerable<BooksCMSViewModel>>> GetAllBooksAsync()
        {
            return Ok(await bookService.GetAllBooksForCMSAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsCMSViewModel>> GetBookByIdAsync(int id)
        {
            return Ok(await bookService.GetBookDetailsForCMSByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> PostBookAsync(BookPostViewModel bookPost)
        {
            if (bookPost == null)
            {
                throw new BadRequestException("Nie można wykonać operacji dodawania, ponieważ przesłany obiekt jest pusty.");
            }
            await bookService.CreateBookAsync(bookPost);

            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookAsync(int id, BookPostViewModel bookPut)
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
