using BookStoreAPI.Services.CMS;
using BookStoreAPI.Services.Statistic;
using BookStoreViewModels.ViewModels.CMS;
using BookStoreViewModels.ViewModels.Statistics;
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
        public async Task<ActionResult<CMSWeeklySummaryViewModel>> GetCMSWeeklySummaryAsync()
        {
            var summary = await cmsService.GetWeeklySummaryOfOrdersRentalsReservationsAsync();
            return Ok(summary);
        }

        [HttpPost]
        [Route("Supply")]
        public async Task<IActionResult> AddNewSupplyAsync()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImRkb2IxMjMiLCJuYW1laWQiOiI2MDExNjM1NC0wMmNhLTQ5MDMtYjczNi05ZTYwNGNjNzhkOTciLCJqdGkiOiIwNjM3ODFkNy0wNjQ0LTQ0YWMtOGFkMC00ZjczNDg2ZDM4NTMiLCJQZXJtaXNzaW9ucyI6WyJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIiwiYm9va3M6ciIsImJvb2tzOnIiLCJib29rczpyIl0sInJvbGUiOiJVc2VyIiwibmJmIjoxNzA3Njk5ODk0LCJleHAiOjIwNjc2OTk4OTQsImlhdCI6MTcwNzY5OTg5NCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI0NyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNDcifQ.JkCb1YoYPzD0sPYQvNHmLKGbwn-4EDPqI0tm3HCAFCs";
            // Oblicz długość tekstu zakodowanego tokenu w bajtach
            int byteCount = System.Text.Encoding.UTF8.GetByteCount(token);

            // Przekształć długość w bajtach na kilobajty
            double kilobytes = byteCount / 1024.0;
            return Ok(kilobytes);
        }

        [HttpGet]
        //[Authorize("CMSRead")]
        [Route("MonthlyRaport")]
        public async Task<ActionResult<StatisticsMonthlyRaportViewModel>> GetMonthlyRaportAsync(int month, int year)
        {
            var stats = await statisticsService.GetMonthlyRaportAsync(month, year);
            return Ok(stats);
        }
    }
}
