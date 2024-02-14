using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Products.Books.Dictionaries;

namespace BookStoreDto.Dtos.Library
{
    public class LibraryItemsDto : BaseDto
    {
        public string FormName { get; set; }
        public string BookTitle { get; set; }
        public string FileFormatName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? ImageURL { get; set; }
        public List<AuthorDto> Authors { get; set; }
    }
}
