using BookStoreAPI.Services.Discounts.Discounts;
using BookStoreViewModels.ViewModels.Products.Discounts;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.BookItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController(IDiscountService discountService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountCMSViewModel>>> GetAllDiscountsAsync()
        {
            var discounts = await discountService.GetAllDiscountsCMSAsync();
            return Ok(discounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountDetailsCMSViewModel>> GetDiscountByIdAsync(int id)
        {
            var discount = await discountService.GetDiscountByIdCMSAsync(id);
            return Ok(discount);
        }

        [HttpPost]
        public async Task<IActionResult> PostDiscountAsync(DiscountCMSPostViewModel discountModel)
        {
            await discountService.CreateDiscountAsync(discountModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscountAsync(int id, DiscountCMSPostViewModel discountModel)
        {
            await discountService.UpdateDiscountAsync(id, discountModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateDiscountAsync(int id)
        {
            await discountService.DeactivateDiscountAsync(id);
            return NoContent();
        }
    }
}
