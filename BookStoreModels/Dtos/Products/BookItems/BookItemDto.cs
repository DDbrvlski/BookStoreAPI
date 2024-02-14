using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Products.Books.Dictionaries;

namespace BookStoreDto.Dtos.Products.BookItems
{
    public class BookItemDto : BaseDto
    {
        public string? ImageURL { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public double Score { get; set; }
        public int FormId { get; set; }
        public string FormName { get; set; }
        public int? FileFormatId { get; set; }
        public string? FileFormatName { get; set; }
        public int? EditionId { get; set; }
        public string? EditionName { get; set; }
        public string AvailabilityName { get; set; }
        public int AvailabilityID { get; set; }
        public decimal DiscountedBruttoPrice { get; set; } = 0;
        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    }
}
