using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Products.BookItems
{
    public class BookItemCarouselDto : BaseDto
    {
        public string? ImageURL { get; set; }
        public string? Title { get; set; }
        public int FormId { get; set; }
        public string FormName { get; set; }
    }
}
