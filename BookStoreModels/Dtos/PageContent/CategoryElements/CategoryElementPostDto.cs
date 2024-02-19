using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.PageContent.CategoryElements
{
    public class CategoryElementPostDto : BaseDto
    {
        [Required]
        public string Path { get; set; }
        [Required]
        public string Logo { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int Position { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
        [Required]
        public int CategoryID { get; set; }
    }
}
