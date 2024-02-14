using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Media.Images;
using BookStoreDto.Dtos.Products.Books.Dictionaries;

namespace BookStoreDto.Dtos.Products.Books
{
    public class BookDetailsCMSDto : BaseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int OriginalLanguageID { get; set; }
        public string OriginalLanguageName { get; set; }
        public int PublisherID { get; set; }
        public string PublisherName { get; set; }
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
        public List<ImageDto> Images { get; set; } = new List<ImageDto>();
    }
}
