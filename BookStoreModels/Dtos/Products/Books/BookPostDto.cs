using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Media.Images;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Products.Books
{
    public class BookPostDto : BaseDto
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public int OriginalLanguageID { get; set; }

        [Required]
        public int PublisherID { get; set; }


        public List<ListOfIds>? ListOfBookAuthors { get; set; }
        public List<ListOfIds>? ListOfBookCategories { get; set; }
        public List<ImageDto>? ListOfBookImages { get; set; }
    }
}
