using BookStoreDto.Dtos.Products.Books.Dictionaries;

namespace BookStoreDto.Dtos.Supply
{
    public class SupplyBooksDto
    {
        public int BookItemId { get; set; }
        public string BookTitle { get; set; }
        public string FormName { get; set; }
        public string? EditionName { get; set; }
        public string? FileFormatName { get; set; }
        public decimal BruttoPrice { get; set; }
        public int Quantity { get; set; }
        public List<AuthorDto> Authors { get; set; }
    }
}
