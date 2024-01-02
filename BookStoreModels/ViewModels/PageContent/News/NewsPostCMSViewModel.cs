using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.PageContent.News
{
    public class NewsPostCMSViewModel : BaseViewModel
    {
        public string? Topic { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
        public string? Content { get; set; }
        public string? AuthorName { get; set; }
    }
}
