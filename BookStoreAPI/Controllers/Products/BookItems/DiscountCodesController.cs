﻿using BookStoreAPI.Services.Discounts.DiscountCodes;
using BookStoreDto.Dtos.Orders;
using BookStoreDto.Dtos.Products.DiscountCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.BookItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCodesController(IDiscountCodeService discountCodeService) : ControllerBase
    {
        [HttpGet]
        [Authorize("DiscountCodesRead")]
        public async Task<ActionResult<IEnumerable<DiscountCodeCMSDto>>> GetAllDiscountCodesAsync()
        {
            var discountCodes = await discountCodeService.GetAllDiscountCodesCMSAsync();
            return Ok(discountCodes);
        }

        [HttpGet("{id}")]
        [Authorize("DiscountCodesRead")]
        public async Task<ActionResult<DiscountCodeDetailsCMSDto>> GetDiscountCodeByIdAsync(int id)
        {
            var discountCode = await discountCodeService.GetDiscountCodeByIdCMSAsync(id);
            return Ok(discountCode);
        }

        [HttpPost]
        [Authorize("DiscountCodesWrite")]
        public async Task<IActionResult> PostDiscountCodeAsync(DiscountCodePostCMSDto discountCodeModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await discountCodeService.CreateDiscountCodeAsync(discountCodeModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize("DiscountCodesEdit")]
        public async Task<IActionResult> PutDiscountCodeAsync(int id, DiscountCodePostCMSDto discountCodeModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await discountCodeService.UpdateDiscountCodeAsync(id, discountCodeModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize("DiscountCodesDelete")]
        public async Task<IActionResult> DeactivateDiscountCodeAsync(int id)
        {
            await discountCodeService.DeactivateDiscountCodeAsync(id);
            return NoContent();
        }
    }
}
