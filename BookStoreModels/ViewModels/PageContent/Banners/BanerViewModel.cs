using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.PageContent.Banners
{
    public class BanerViewModel : BaseViewModel
    {
        public string? Path { get; set; }
        public string? Title { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
    }
}
