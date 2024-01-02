using BookStoreAPI.Services.PageElements;
using BookStoreViewModels.ViewModels.PageContent.Banners;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController(IBannerService bannerService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<BanerViewModel>> GetBannerByIdAsync(int id)
        {
            var banner = await bannerService.GetBannerByIdAsync(id);
            return Ok(banner);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BanerViewModel>>> GetAllBannersAsync()
        {
            var banners = await bannerService.GetAllBannersAsync();
            return Ok(banners);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBannerAsync(BanerViewModel bannerModel)
        {
            await bannerService.CreateBannerAsync(bannerModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditBannerAsync(int bannerId, BanerViewModel bannerModel)
        {
            await bannerService.EditBannerAsync(bannerId, bannerModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateBannerAsync(int bannerId)
        {
            await bannerService.DeactivateBannerAsync(bannerId);
            return NoContent();
        }
    }
}
