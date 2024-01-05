using BookStoreAPI.Services.CMS;
using BookStoreViewModels.ViewModels.CMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.CMS
{
    [Route("api/[controller]")]
    [ApiController]
    public class CMSController(ICMSService cmsService) : ControllerBase
    {
        [HttpGet]
        [Route("WeeklySummary")]
        public async Task<ActionResult<CMSWeeklySummaryViewModel>> GetCMSWeeklySummaryAsync()
        {
            var summary = await cmsService.GetWeeklySummaryOfOrdersRentalsReservationsAsync();
            return Ok(summary);
        }
    }
}
