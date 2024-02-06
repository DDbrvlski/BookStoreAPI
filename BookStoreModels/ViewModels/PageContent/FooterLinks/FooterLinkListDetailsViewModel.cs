using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.PageContent.FooterLinks
{
    public class FooterLinkListDetailsViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string URL { get; set; }
        public int Position { get; set; }
    }
}
