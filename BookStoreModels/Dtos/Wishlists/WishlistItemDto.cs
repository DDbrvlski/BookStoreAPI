using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Products.Books.Dictionaries;

namespace BookStoreDto.Dtos.Wishlists
{
    public class WishlistItemDto : BaseDto
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public string? FileFormatName { get; set; }
        public string BookTitle { get; set; }
        public string? EditionName { get; set; }
        public decimal BruttoPrice { get; set; }
        public decimal DiscountedBruttoPrice { get; set; } = 0;
        public string? ImageURL { get; set; }
        public List<AuthorDto> authors { get; set; } = new List<AuthorDto>();
    }
}
