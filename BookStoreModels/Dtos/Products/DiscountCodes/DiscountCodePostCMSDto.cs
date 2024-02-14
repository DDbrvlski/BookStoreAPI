using BookStoreDto.Dtos.Helpers;
namespace BookStoreDto.Dtos.Products.DiscountCodes
{
    public class DiscountCodePostCMSDto : BaseDto
    {
        public decimal PercentOfDiscount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime StartingDate { get; set; }
        public string? Description { get; set; }
        public string Code { get; set; }
    }
}
