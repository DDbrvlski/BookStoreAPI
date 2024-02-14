namespace BookStoreDto.Dtos.PageContent.FooterLinks
{
    public class FooterColumnDetailsDto 
    {
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public int ColumnPosition { get; set; }
        public string HTMLObject { get; set; }

        public List<FooterLinkListDetailsDto>? FooterLinksList { get; set; }
    }
}
