using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Products.DiscountCodes
{
    public class DiscountCodeCMSDto : BaseDto
    {
        public string Code { get; set; }
        public decimal PercentOfDiscount { get; set; }
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime ExpiryDate { get; set;}
    }
}
