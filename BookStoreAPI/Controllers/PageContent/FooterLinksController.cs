using BookStoreAPI.Services.PageElements;
using BookStoreData.Models.PageContent;
using BookStoreViewModels.ViewModels.PageContent.FooterLinks;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooterLinksController(IFooterLinkService footerLinkService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FooterLinkViewModel>>> GetAllFooterLinksAsync()
        {
            var footerLinks = await footerLinkService.GetAllFooterLinksAsync();
            return Ok(footerLinks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FooterLinkViewModel>> GetFooterLinkByIdAsync(int id)
        {
            var footerLink = await footerLinkService.GetFooterLinkByIdAsync(id);
            return Ok(footerLink);
        }

        [HttpGet("Column/{id}")]
        public async Task<ActionResult<FooterColumnDetailsViewModel>> GetFooterLinksInColumnByColumnIdAsync(int id)
        {
            var footerColumn = await footerLinkService.GetFooterLinksInColumnByColumnIdAsync(id);
            return Ok(footerColumn);
        }

        [HttpGet("Column")]
        public async Task<ActionResult<IEnumerable<FooterColumnDetailsViewModel>>> GetFooterLinksInColumnsAsync()
        {
            var footerColumns = await footerLinkService.GetFooterLinksInColumnsAsync();
            return Ok(footerColumns);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFooterLinkAsync(FooterLinks footerLinkModel)
        {
            await footerLinkService.CreateFooterLinkAsync(footerLinkModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditFooterLinkAsync(int id, FooterLinks footerLinkModel)
        {
            await footerLinkService.EditFooterLinkAsync(id, footerLinkModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateFooterLinkAsync(int id)
        {
            await footerLinkService.DeactivateFooterLinkAsync(id);
            return NoContent();
        }
    }
}
