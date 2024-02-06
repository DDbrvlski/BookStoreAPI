using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.Discounts
{
    public class DiscountCMSPostViewModel : BaseViewModel
    {
        public decimal PercentOfDiscount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime StartingDate { get; set; }
        public string? Description { get; set; }
        public string Title { get; set; }
        public List<ListOfIds>? ListOfBookItems { get; set; }
    }
}
