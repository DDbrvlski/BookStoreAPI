using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Products.Discounts
{
    public class DiscountCMSPostDto : BaseDto
    {
        [Required]
        public decimal PercentOfDiscount { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public DateTime StartingDate { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Title { get; set; }
        public List<ListOfIds>? ListOfBookItems { get; set; }
    }
}
