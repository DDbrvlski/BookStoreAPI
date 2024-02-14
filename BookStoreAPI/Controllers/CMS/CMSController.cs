using BookStoreAPI.Services.CMS;
using BookStoreAPI.Services.Statistic;
using BookStoreDto.Dtos.CMS;
using BookStoreDto.Dtos.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.CMS
{
    [Route("api/[controller]")]
    [ApiController]
    public class CMSController(ICMSService cmsService, IStatisticsService statisticsService) : ControllerBase
    {
        [HttpGet]
        [Authorize("CMSRead")]
        [Route("WeeklySummary")]
        public async Task<ActionResult<CMSWeeklySummaryDto>> GetCMSWeeklySummaryAsync()
        {
            var summary = await cmsService.GetWeeklySummaryOfOrdersRentalsReservationsAsync();
            return Ok(summary);
        }

        [HttpGet]
        [Authorize("CMSRead")]
        [Route("MonthlyRaport")]
        public async Task<ActionResult<StatisticsMonthlyRaportDto>> GetMonthlyRaportAsync(int month, int year)
        {
            var stats = await statisticsService.GetMonthlyRaportAsync(month, year);
            return Ok(stats);
        }
    }
}
