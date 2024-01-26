using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderItemDetailsViewModel : BaseViewModel
    {
        public string? FormName { get; set; }
        public string? BookTitle { get; set; }
        public string? EditionName { get; set; }
        public string? FileFormatName { get; set; }
        public decimal? BruttoPrice { get; set; }
        public decimal? TotalBruttoPrice { get; set; }
        public int? Quantity { get; set; }
        public string? ImageURL { get; set; }
        public List<AuthorViewModel> Authors { get; set; }
    }
}
