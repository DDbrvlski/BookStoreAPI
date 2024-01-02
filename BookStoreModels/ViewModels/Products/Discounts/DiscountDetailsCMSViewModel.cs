using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Products.BookItems;

namespace BookStoreViewModels.ViewModels.Products.Discounts
{
    public class DiscountDetailsCMSViewModel : BaseViewModel
    {
        public decimal PercentOfDiscount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime StartingDate { get; set; }
        public string? Description { get; set; }
        public string? Title { get; set; }
        public bool IsAvailable { get; set; }
        public List<BookItemCMSViewModel>? ListOfBookItems { get; set; }
    }
}
