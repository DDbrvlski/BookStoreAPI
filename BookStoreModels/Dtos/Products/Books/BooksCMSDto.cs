using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Products.Books.Dictionaries;

namespace BookStoreDto.Dtos.Products.Books
{
    public class BooksCMSDto : BaseDto
    {
        public string Title { get; set; }
        public string PublisherName { get; set; }
        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    }
}
