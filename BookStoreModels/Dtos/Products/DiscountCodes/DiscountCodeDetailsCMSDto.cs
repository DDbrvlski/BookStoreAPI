using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Products.BookItems;

namespace BookStoreDto.Dtos.Products.DiscountCodes
{
    public class DiscountCodeDetailsCMSDto : BaseDto
    {
        public string Code { get; set; }
        public decimal PercentOfDiscount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime StartingDate { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
    }
}
