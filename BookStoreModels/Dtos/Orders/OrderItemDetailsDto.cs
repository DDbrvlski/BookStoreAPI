using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Products.Books.Dictionaries;

namespace BookStoreDto.Dtos.Orders
{
    public class OrderItemDetailsDto : BaseDto
    {
        public string FormName { get; set; }
        public string BookTitle { get; set; }
        public string? EditionName { get; set; }
        public string? FileFormatName { get; set; }
        public decimal BruttoPrice { get; set; }
        public decimal TotalBruttoPrice { get; set; }
        public int Quantity { get; set; }
        public string? ImageURL { get; set; }
        public List<AuthorDto> Authors { get; set; }
    }
}
