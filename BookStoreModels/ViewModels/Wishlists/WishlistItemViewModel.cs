using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;

namespace BookStoreViewModels.ViewModels.Wishlists
{
    public class WishlistItemViewModel : BaseViewModel
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public string? FileFormatName { get; set; }
        public string BookTitle { get; set; }
        public string? EditionName { get; set; }
        public decimal BruttoPrice { get; set; }
        public decimal DiscountedBruttoPrice { get; set; } = 0;
        public string? ImageURL { get; set; }
        public List<AuthorViewModel> authors { get; set; } = new List<AuthorViewModel>();
    }
}
