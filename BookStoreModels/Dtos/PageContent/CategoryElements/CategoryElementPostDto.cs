using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.PageContent.CategoryElements
{
    public class CategoryElementPostDto : BaseDto
    {
        public string Path { get; set; }
        public string Logo { get; set; }
        public string Content { get; set; }
        public int Position { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
        public int CategoryID { get; set; }
    }
}
