using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderItemDetailsViewModel : BaseViewModel
    {
        public string? FormName { get; set; }
        public string? BookTitle { get; set; }
        public string? EditionName { get; set; }
        public decimal? PriceBrutto { get; set; }
        public decimal? FullPriceBrutto { get; set; }
        public int? Quantity { get; set; }
        public string? ImageURL { get; set; }
    }
}
