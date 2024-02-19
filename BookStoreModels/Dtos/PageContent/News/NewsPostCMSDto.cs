using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.PageContent.News
{
    public class NewsPostCMSDto : BaseDto
    {
        [Required]
        public string Topic { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
