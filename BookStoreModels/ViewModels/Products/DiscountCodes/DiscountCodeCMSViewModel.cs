using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.DiscountCodes
{
    public class DiscountCodeCMSViewModel : BaseViewModel
    {
        public string Code { get; set; }
        public decimal PercentOfDiscount { get; set; }
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }
    }
}
