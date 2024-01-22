using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;

namespace BookStoreViewModels.ViewModels.Library
{
    public class LibraryItemsViewModel : BaseViewModel
    {
        public string? FormName { get; set; }
        public string? BookTitle { get; set; }
        public string? FileFormatName { get; set; }
        public string? ImageURL { get; set; }
        public List<AuthorViewModel> Authors { get; set; }
    }
}
