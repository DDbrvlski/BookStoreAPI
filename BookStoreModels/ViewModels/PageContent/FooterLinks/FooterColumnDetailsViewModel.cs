namespace BookStoreViewModels.ViewModels.PageContent.FooterLinks
{
    public class FooterColumnDetailsViewModel 
    {
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public int ColumnPosition { get; set; }
        public string HTMLObject { get; set; }

        public List<FooterLinkListDetailsViewModel>? FooterLinksList { get; set; }
    }
}
