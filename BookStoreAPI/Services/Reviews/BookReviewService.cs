using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Customers;
using BookStoreBusinessLogic.BusinessLogic.BookReviews;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreDto.Dtos.Products.BookItems.Review;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Reviews
{
    public interface IBookReviewService
    {
        Task CreateBookReview(BookReviewPostDto bookReviewModel);
        Task<IEnumerable<BookReviewDto>> GetAllBookReviewsByBookItemIdAsync(int bookItemId, int numberOfElements);
        Task<Dictionary<int, int>> GetBookItemReviewScoresAsync(int bookItemId);
        Task<BookReviewPostDto> GetExistingUserBookReviewByBookItemIdAsync(int bookItemId);
    }

    public class BookReviewService(BookStoreContext context, IBookReviewLogic bookReviewLogic, ICustomerService customerService) : IBookReviewService
    {
        public async Task<IEnumerable<BookReviewDto>> GetAllBookReviewsByBookItemIdAsync(int bookItemId, int numberOfElements)
        {
            return await context.BookItemReview
                        .Include(x => x.Customer)
                        .Include(x => x.Score)
                        .Where(x => x.IsActive && x.BookItemID == bookItemId)
                        .Select(x => new BookReviewDto()
                        {
                            Id = x.Id,
                            Content = x.Content,
                            CreationDate = x.CreationDate,
                            CustomerName = x.Customer.Name + " " + x.Customer.Surname,
                            ScoreValue = x.Score.Value
                        }).Take(numberOfElements).ToListAsync();
        }
        public async Task<BookReviewPostDto> GetExistingUserBookReviewByBookItemIdAsync(int bookItemId)
        {
            var customer = await customerService.GetCustomerByTokenAsync();
            var bookReview = await context.BookItemReview
                .FirstOrDefaultAsync(x => x.IsActive && x.CustomerID == customer.Id && x.BookItemID == bookItemId);

            return new BookReviewPostDto()
            {
                Content = bookReview.Content,
                BookItemId = bookReview.BookItemID,
                Id = bookReview.Id,
                ScoreId = bookReview.ScoreID
            };
        }
        public async Task CreateBookReview(BookReviewPostDto bookReviewModel)
        {
            var customer = await customerService.GetCustomerByTokenAsync();
            var existingBookReview = await context.BookItemReview
                .FirstOrDefaultAsync(x => x.IsActive && x.CustomerID == customer.Id && x.BookItemID == bookReviewModel.BookItemId);

            if (existingBookReview != null)
            {
                existingBookReview.Content = bookReviewModel.Content;
                existingBookReview.ScoreID = bookReviewModel.ScoreId;
            }
            else
            {
                var review = new BookItemReview()
                {
                    Content = bookReviewModel.Content,
                    BookItemID = bookReviewModel.BookItemId,
                    ScoreID = bookReviewModel.ScoreId,
                    CustomerID = customer.Id
                };

                await context.BookItemReview.AddAsync(review);
            }
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            await UpdateBookItemScoreAsync((int)bookReviewModel.BookItemId);
        }
        public async Task<Dictionary<int, int>> GetBookItemReviewScoresAsync(int bookItemId)
        {
            var scoreOccurrences = await context.BookItemReview
                .Where(x => x.IsActive && x.BookItemID == bookItemId)
                .GroupBy(review => review.Score.Value)
                .ToDictionaryAsync(group => group.Key, group => group.Count());

            return bookReviewLogic.CalculateAllScoreOccurrences(scoreOccurrences);
        }

        private async Task UpdateBookItemScoreAsync(int bookItemId)
        {
            var listOfScores = await context.BookItemReview
                .Where(x => x.IsActive && x.BookItemID == bookItemId).Select(x => x.Score.Value)
                .ToListAsync();

            if (listOfScores.Any())
            {
                var scoreToUpdate = await context.BookItem.FirstAsync(x => x.Id == bookItemId);
                scoreToUpdate.Score = listOfScores.Average();

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
    }
}
