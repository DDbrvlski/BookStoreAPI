using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Products.Discounts
{
    public class DiscountCMSDto : BaseDto
    {
        public string Title { get; set; }
        public decimal PercentOfDiscount { get; set; }
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }
    }
}
