using BookStoreAPI.Services.PageElements;
using BookStoreData.Data;
using BookStoreData.Models.PageContent;
using BookStoreViewModels.ViewModels.PageContent.Banners;
using BookStoreViewModels.ViewModels.PageContent.CategoryElements;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryElementsController(ICategoryElementService categoryElementService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryElementViewModel>> GetCategoryElementByIdAsync(int id)
        {
            var categoryElement = await categoryElementService.GetCategoryElementByIdAsync(id);
            return Ok(categoryElement);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryElementViewModel>>> GetAllCategoryElementsAsync()
        {
            var categoryElement = await categoryElementService.GetAllCategoryElementsAsync();
            return Ok(categoryElement);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryElementAsync(CategoryElementViewModel categoryElementModel)
        {
            await categoryElementService.CreateCategoryElementAsync(categoryElementModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCategoryElementAsync(int id, CategoryElementViewModel categoryElementModel)
        {
            await categoryElementService.EditCategoryElementAsync(id, categoryElementModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateCategoryElementAsync(int categoryElementId)
        {
            await categoryElementService.DeactivateCategoryElementAsync(categoryElementId);
            return NoContent();
        }
    }
}
