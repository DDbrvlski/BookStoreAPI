using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;

namespace BookStoreViewModels.ViewModels.Products.BookItems
{
    public class BookItemInOrderViewModel : BaseViewModel
    {
        public string? Title { get; set; }
        public string? ImageURL { get; set; }
        public List<AuthorViewModel>? AuthorsName { get; set; }
    }
}
