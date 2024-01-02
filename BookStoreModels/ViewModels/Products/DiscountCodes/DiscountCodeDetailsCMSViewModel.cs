using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Products.BookItems;

namespace BookStoreViewModels.ViewModels.Products.DiscountCodes
{
    public class DiscountCodeDetailsCMSViewModel : BaseViewModel
    {
        public string Code { get; set; }
        public decimal PercentOfDiscount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime StartingDate { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
    }
}
