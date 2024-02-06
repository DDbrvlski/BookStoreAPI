using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Media.Images
{
    public class ImageViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public int Position { get; set; }
    }
}
