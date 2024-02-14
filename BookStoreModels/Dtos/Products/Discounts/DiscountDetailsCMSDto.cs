using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Products.BookItems;

namespace BookStoreDto.Dtos.Products.Discounts
{
    public class DiscountDetailsCMSDto : BaseDto
    {
        public decimal PercentOfDiscount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime StartingDate { get; set; }
        public string? Description { get; set; }
        public string Title { get; set; }
        public bool IsAvailable { get; set; }
        public List<BookItemCMSDto>? ListOfBookItems { get; set; }
    }
}
