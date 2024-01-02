using BookStoreViewModels.ViewModels.Helpers;
namespace BookStoreViewModels.ViewModels.Products.DiscountCodes
{
    public class DiscountCodePostCMSViewModel : BaseViewModel
    {
        public decimal PercentOfDiscount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime StartingDate { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
    }
}
