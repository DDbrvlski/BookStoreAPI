using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.PageContent.DiscountBanners
{
    public class DiscountBannerDto : BaseDto
    {
        [Required]
        public string Header { get; set; }
        [Required]
        public string ButtonTitle { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
    }
}
