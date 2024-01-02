using BookStoreAPI.Services.Reviews;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Products.BookItems.Review;
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
        public async Task<ActionResult<BookReviewPostViewModel>> GetUserBookReviewByBookItemIdAsync(int bookItemId)
        {
            var bookReview = await bookReviewService.GetExistingUserBookReviewByBookItemIdAsync(bookItemId);
            return Ok(bookReview);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReviewViewModel>>> GetAllBookReviewsByBookItemIdAsync(int bookItemId, int numberOfElements = 4)
        {
            var bookReviews = await bookReviewService.GetAllBookReviewsByBookItemIdAsync(bookItemId, numberOfElements);
            return Ok(bookReviews);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> PostBookReview(BookReviewPostViewModel bookReviewModel)
        {
            await bookReviewService.CreateBookReview(bookReviewModel);
            return NoContent();
        }
    }
}
