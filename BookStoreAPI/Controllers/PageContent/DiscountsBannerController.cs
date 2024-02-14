using BookStoreAPI.Services.PageElements;
using BookStoreDto.Dtos.PageContent.DiscountBanners;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsBannerController(IDiscountBannerService discountBannerService) : ControllerBase
    {
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DiscountBannerDto>> GetDiscountBannerByIdAsync(int id)
        {
            var discountbanner = await discountBannerService.GetDiscountBannerByIdAsync(id);
            return Ok(discountbanner);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DiscountBannerDto>>> GetAllDiscountBannersAsync()
        {
            var discountbanners = await discountBannerService.GetAllDiscountBannersAsync();
            return Ok(discountbanners);
        }

        [HttpPost]
        [Authorize("DiscountBannerWrite")]
        public async Task<IActionResult> CreateDiscountBannerAsync(DiscountBannerDto discountBannerDto)
        {
            await discountBannerService.CreateDiscountBannerAsync(discountBannerDto);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize("DiscountBannerEdit")]
        public async Task<IActionResult> EditDiscountBannerAsync(int id, DiscountBannerDto discountBannerDto)
        {
            await discountBannerService.EditDiscountBannerAsync(id, discountBannerDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize("DiscountBannerDelete")]
        public async Task<IActionResult> DeactivateDiscountBannerAsync(int id)
        {
            await discountBannerService.DeactivateDiscountBannerAsync(id);
            return NoContent();
        }
    }
}
