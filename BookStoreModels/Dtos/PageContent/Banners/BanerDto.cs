using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.PageContent.Banners
{
    public class BanerDto : BaseDto
    {
        [Required]
        public string Path { get; set; }
        [Required]
        public string Title { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
    }
}
