using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Products.Books.Dictionaries;

namespace BookStoreDto.Dtos.Products.BookItems
{
    public class BookItemInOrderDto : BaseDto
    {
        public string Title { get; set; }
        public string? ImageURL { get; set; }
        public List<AuthorDto>? AuthorsName { get; set; }
    }
}
