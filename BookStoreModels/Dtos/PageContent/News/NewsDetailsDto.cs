using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.PageContent.News
{
    public class NewsDetailsDto : BaseDto
    {
        public string Topic { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
