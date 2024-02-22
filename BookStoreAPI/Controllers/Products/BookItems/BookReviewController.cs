using BookStoreAPI.Services.Reviews;
using BookStoreData.Models.Accounts;
using BookStoreDto.Dtos.Products.BookItems.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.BookItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookItemReviewController(IBookReviewService bookReviewService) : ControllerBase
    {
        [HttpGet("{bookItemId}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<BookReviewPostDto?>> GetUserBookReviewByBookItemIdAsync(int bookItemId)
        {
            var bookReview = await bookReviewService.GetExistingUserBookReviewByBookItemIdAsync(bookItemId);
            return Ok(bookReview);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReviewDto>>> GetAllBookReviewsByBookItemIdAsync(int bookItemId, int? numberOfElements)
        {
            var bookReviews = await bookReviewService.GetAllBookReviewsByBookItemIdAsync(bookItemId, numberOfElements);
            return Ok(bookReviews);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> PostBookReview(BookReviewPostDto bookReviewModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await bookReviewService.CreateBookReview(bookReviewModel);
            return NoContent();
        }
    }
}
