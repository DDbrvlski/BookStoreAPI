using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Products.BookItems.Review
{
    public class BookReviewDto : BaseDto
    {
        public string CustomerName { get; set; }
        public int ScoreValue { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Content { get; set; }
    }
}
