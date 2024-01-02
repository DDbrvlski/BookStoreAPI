using BookStoreAPI.Services.Stock;
using BookStoreViewModels.ViewModels.Products.StockAmount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers.Products.BookItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockAmountController(IStockAmountService stockAmountService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockAmountViewModel>>> GetAllStockAmountAsync()
        {
            var stockAmounts = await stockAmountService.GetAllStockAmountAsync();
            return Ok(stockAmounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockAmountViewModel>> GetStockAmountByIdAsync(int id)
        {
            var stockAmount = await stockAmountService.GetStockAmountByIdAsync(id);
            return Ok(stockAmount);
        }
    }
}
