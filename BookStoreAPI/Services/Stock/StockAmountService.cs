using BookStoreData.Data;
using BookStoreViewModels.ViewModels.Products.StockAmount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Stock
{
    public interface IStockAmountService
    {
        Task<ActionResult<IEnumerable<StockAmountViewModel>>> GetAllStockAmountAsync();
        Task<ActionResult<StockAmountViewModel?>> GetStockAmountByIdAsync(int id);
    }

    public class StockAmountService(BookStoreContext context) : IStockAmountService
    {
        public async Task<ActionResult<IEnumerable<StockAmountViewModel>>> GetAllStockAmountAsync()
        {
            return await context.StockAmount
                .Where(x => x.IsActive == true)
                .Select(x => new StockAmountViewModel
                {
                    Id = x.Id,
                    BookTitle = x.BookItem.Book.Title,
                    Amount = x.Amount,
                    BookItemID = x.BookItemID
                })
                .ToListAsync();
        }

        public async Task<ActionResult<StockAmountViewModel?>> GetStockAmountByIdAsync(int id)
        {
            return await context.StockAmount
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new StockAmountViewModel()
                {
                    Id = x.Id,
                    BookTitle = x.BookItem.Book.Title,
                    Amount = x.Amount,
                    BookItemID = x.BookItemID
                }).FirstAsync();
        }
    }
}
