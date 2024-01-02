using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Media.Images;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;

namespace BookStoreViewModels.ViewModels.Products.Books
{
    public class BookDetailsCMSViewModel : BaseViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? OriginalLanguageID { get; set; }
        public string? OriginalLanguageName { get; set; }
        public int? PublisherID { get; set; }
        public string? PublisherName { get; set; }
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<AuthorViewModel> Authors { get; set; } = new List<AuthorViewModel>();
        public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();
    }
}
