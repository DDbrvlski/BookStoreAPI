using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.PageContent.CategoryElements
{
    public class CategoryElementDto : CategoryElementPostDto
    {
        public string CategoryName { get; set; }
    }
}
