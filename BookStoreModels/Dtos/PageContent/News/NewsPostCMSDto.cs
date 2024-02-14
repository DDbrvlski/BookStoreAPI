using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.PageContent.News
{
    public class NewsPostCMSDto : BaseDto
    {
        public string Topic { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
        public string Content { get; set; }
    }
}
