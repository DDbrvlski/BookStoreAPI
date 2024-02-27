using BookStoreAPI.Services.Stock;
using BookStoreDto.Dtos.Products.StockAmount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.BookItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockAmountController(IStockAmountService stockAmountService) : ControllerBase
    {
        [HttpGet]
        [Authorize("StockAmountRead")]
        public async Task<ActionResult<IEnumerable<StockAmountDto>>> GetAllStockAmountAsync()
        {
            var stockAmounts = await stockAmountService.GetAllStockAmountAsync();
            return Ok(stockAmounts);
        }

        [HttpGet("{id}")]
        [Authorize("StockAmountRead")]
        public async Task<ActionResult<StockAmountDto>> GetStockAmountByIdAsync(int id)
        {
            var stockAmount = await stockAmountService.GetStockAmountByIdAsync(id);
            return Ok(stockAmount);
        }
    }
}
