using BookStoreAPI.Services.PageElements;
using BookStoreViewModels.ViewModels.PageContent.DiscountBanners;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsBannerController(IDiscountBannerService discountBannerService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountBannerViewModel>> GetDiscountBannerByIdAsync(int id)
        {
            var discountbanner = await discountBannerService.GetDiscountBannerByIdAsync(id);
            return Ok(discountbanner);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountBannerViewModel>>> GetAllDiscountBannersAsync()
        {
            var discountbanners = await discountBannerService.GetAllDiscountBannersAsync();
            return Ok(discountbanners);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscountBannerAsync(DiscountBannerViewModel discountBannerViewModel)
        {
            await discountBannerService.CreateDiscountBannerAsync(discountBannerViewModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDiscountBannerAsync(int id, DiscountBannerViewModel discountBannerViewModel)
        {
            await discountBannerService.EditDiscountBannerAsync(id, discountBannerViewModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateDiscountBannerAsync(int id)
        {
            await discountBannerService.DeactivateDiscountBannerAsync(id);
            return NoContent();
        }
    }
}
