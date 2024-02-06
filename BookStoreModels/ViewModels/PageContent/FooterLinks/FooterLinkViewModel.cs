using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.PageContent.FooterLinks
{
    public class FooterLinkViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string URL { get; set; }
        public int Position { get; set; }
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public int ColumnPosition { get; set; }
        public string HTMLObject { get; set; }

    }
}
