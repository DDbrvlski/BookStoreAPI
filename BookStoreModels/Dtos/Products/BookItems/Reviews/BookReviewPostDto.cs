using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Products.BookItems.Review
{
    public class BookReviewPostDto : BaseDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int ScoreId { get; set; }
        [Required]
        public int BookItemId { get; set; }
    }
}
