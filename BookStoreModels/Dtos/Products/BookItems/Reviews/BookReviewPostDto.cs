using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Products.BookItems.Review
{
    public class BookReviewPostDto : BaseDto
    {
        public string Content { get; set; }
        public int ScoreId { get; set; }
        public int BookItemId { get; set; }
    }
}
