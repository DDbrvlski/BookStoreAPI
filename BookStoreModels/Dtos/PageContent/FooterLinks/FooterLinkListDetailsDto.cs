using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.PageContent.FooterLinks
{
    public class FooterLinkListDetailsDto : BaseDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string URL { get; set; }
        public int Position { get; set; }
    }
}
