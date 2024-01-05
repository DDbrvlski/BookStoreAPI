using BookStoreAPI.Services.Discounts.DiscountCodes;
using BookStoreViewModels.ViewModels.Orders;
using BookStoreViewModels.ViewModels.Products.DiscountCodes;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.BookItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCodesController(IDiscountCodeService discountCodeService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountCodeCMSViewModel>>> GetAllDiscountCodesAsync()
        {
            var discountCodes = await discountCodeService.GetAllDiscountCodesCMSAsync();
            return Ok(discountCodes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountCodeDetailsCMSViewModel>> GetDiscountCodeByIdAsync(int id)
        {
            var discountCode = await discountCodeService.GetDiscountCodeByIdCMSAsync(id);
            return Ok(discountCode);
        }

        [HttpPost]
        public async Task<IActionResult> PostDiscountCodeAsync(DiscountCodePostCMSViewModel discountCodeModel)
        {
            await discountCodeService.CreateDiscountCodeAsync(discountCodeModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscountCodeAsync(int id, DiscountCodePostCMSViewModel discountCodeModel)
        {
            await discountCodeService.UpdateDiscountCodeAsync(id, discountCodeModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateDiscountCodeAsync(int id)
        {
            await discountCodeService.DeactivateDiscountCodeAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Route("Order")]
        public async Task<ActionResult<OrderDiscountCheckViewModel>> ApplyDiscountForOrder(OrderDiscountCheckViewModel orderDiscountModel)
        {
            var orderDiscount = await discountCodeService.ApplyDiscountCodeToOrderAsync(orderDiscountModel);
            return Ok(orderDiscount);
        }
    }
}
