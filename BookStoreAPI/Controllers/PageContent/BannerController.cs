using BookStoreAPI.Services.PageElements;
using BookStoreViewModels.ViewModels.PageContent.Banners;
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
        public async Task<ActionResult<BanerViewModel>> GetBannerByIdAsync(int id)
        {
            var banner = await bannerService.GetBannerByIdAsync(id);
            return Ok(banner);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BanerViewModel>>> GetAllBannersAsync()
        {
            var banners = await bannerService.GetAllBannersAsync();
            return Ok(banners);
        }

        [HttpPost]
        [Authorize("BannerWrite")]
        public async Task<IActionResult> CreateBannerAsync(BanerViewModel bannerModel)
        {
            await bannerService.CreateBannerAsync(bannerModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize("BannerEdit")]
        public async Task<IActionResult> EditBannerAsync(int id, BanerViewModel bannerModel)
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
