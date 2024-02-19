using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;
namespace BookStoreDto.Dtos.Products.DiscountCodes
{
    public class DiscountCodePostCMSDto : BaseDto
    {
        [Required]
        public decimal PercentOfDiscount { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public DateTime StartingDate { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
