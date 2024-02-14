﻿using BookStoreAPI.Services.CMS;
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

        [HttpGet]
        [Authorize("CMSRead")]
        [Route("MonthlyRaport")]
        public async Task<ActionResult<StatisticsMonthlyRaportViewModel>> GetMonthlyRaportAsync(int month, int year)
        {
            var stats = await statisticsService.GetMonthlyRaportAsync(month, year);
            return Ok(stats);
        }
    }
}
