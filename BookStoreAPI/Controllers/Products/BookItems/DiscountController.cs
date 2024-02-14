using BookStoreAPI.Services.Discounts.Discounts;
using BookStoreViewModels.ViewModels.Products.Discounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.BookItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController(IDiscountService discountService) : ControllerBase
    {
        [HttpGet]
        [Authorize("DiscountRead")]
        public async Task<ActionResult<IEnumerable<DiscountCMSViewModel>>> GetAllDiscountsAsync()
        {
            var discounts = await discountService.GetAllDiscountsCMSAsync();
            return Ok(discounts);
        }

        [HttpGet("{id}")]
        [Authorize("DiscountRead")]
        public async Task<ActionResult<DiscountDetailsCMSViewModel>> GetDiscountByIdAsync(int id)
        {
            var discount = await discountService.GetDiscountByIdCMSAsync(id);
            return Ok(discount);
        }

        [HttpPost]
        [Authorize("DiscountWrite")]
        public async Task<IActionResult> PostDiscountAsync(DiscountCMSPostViewModel discountModel)
        {
            await discountService.CreateDiscountAsync(discountModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize("DiscountEdit")]
        public async Task<IActionResult> PutDiscountAsync(int id, DiscountCMSPostViewModel discountModel)
        {
            await discountService.UpdateDiscountAsync(id, discountModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize("DiscountDelete")]
        public async Task<IActionResult> DeactivateDiscountAsync(int id)
        {
            await discountService.DeactivateDiscountAsync(id);
            return NoContent();
        }
    }
}
