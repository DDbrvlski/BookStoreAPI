using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.PageContent.Banners
{
    public class BanerDto : BaseDto
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
    }
}
