using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;

namespace BookStoreViewModels.ViewModels.Products.Books
{
    public class BooksCMSViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public string PublisherName { get; set; }
        public List<AuthorViewModel> Authors { get; set; } = new List<AuthorViewModel>();
    }
}
