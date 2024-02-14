using BookStoreAPI.Services.PageElements;
using BookStoreDto.Dtos.PageContent.Banners;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController(IBannerService bannerService) : ControllerBase
    {
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BanerDto>> GetBannerByIdAsync(int id)
        {
            var banner = await bannerService.GetBannerByIdAsync(id);
            return Ok(banner);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BanerDto>>> GetAllBannersAsync()
        {
            var banners = await bannerService.GetAllBannersAsync();
            return Ok(banners);
        }

        [HttpPost]
        [Authorize("BannerWrite")]
        public async Task<IActionResult> CreateBannerAsync(BanerDto bannerModel)
        {
            await bannerService.CreateBannerAsync(bannerModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize("BannerEdit")]
        public async Task<IActionResult> EditBannerAsync(int id, BanerDto bannerModel)
        {
            await bannerService.EditBannerAsync(id, bannerModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize("BannerDelete")]
        public async Task<IActionResult> DeactivateBannerAsync(int id)
        {
            await bannerService.DeactivateBannerAsync(id);
            return NoContent();
        }
    }
}
