using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.PageContent.DiscountBanners
{
    public class DiscountBannerDto : BaseDto
    {
        public string Header { get; set; }
        public string ButtonTitle { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
    }
}
