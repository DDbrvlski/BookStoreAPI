using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Media.Images
{
    public class ImageDto : BaseDto
    {
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public int Position { get; set; }
    }
}
