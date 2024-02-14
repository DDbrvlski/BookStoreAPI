using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Products.Discounts
{
    public class DiscountCMSPostDto : BaseDto
    {
        public decimal PercentOfDiscount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime StartingDate { get; set; }
        public string? Description { get; set; }
        public string Title { get; set; }
        public List<ListOfIds>? ListOfBookItems { get; set; }
    }
}
