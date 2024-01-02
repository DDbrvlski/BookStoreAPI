using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.BookItems
{
    public class BookItemCarouselViewModel : BaseViewModel
    {
        public string? ImageURL { get; set; }
        public string? Title { get; set; }
        public int? FormId { get; set; }
        public string? FormName { get; set; }
    }
}
