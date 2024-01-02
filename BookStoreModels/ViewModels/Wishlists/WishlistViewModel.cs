using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Wishlists
{
    public class WishlistViewModel : BaseViewModel
    {
        public decimal? FullPrice { get; set; }
        public bool IsPublic { get; set; }
        public List<WishlistItemViewModel>? Items { get; set; }
    }
}
