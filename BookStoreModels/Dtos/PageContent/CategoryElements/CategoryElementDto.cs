using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.PageContent.CategoryElements
{
    public class CategoryElementDto : CategoryElementPostDto
    {
        [Required]
        public string CategoryName { get; set; }
    }
}
