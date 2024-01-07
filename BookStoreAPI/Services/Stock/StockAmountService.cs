using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreViewModels.ViewModels.Products.StockAmount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Stock
{
    public interface IStockAmountService
    {
        Task CreateStockAmountAsync(int bookItemId, int? amount);
        Task DeactivateStockAmountAsync(int bookItemId);
        Task<ActionResult<IEnumerable<StockAmountViewModel>>> GetAllStockAmountAsync();
        Task<ActionResult<StockAmountViewModel?>> GetStockAmountByIdAsync(int id);
        Task UpdateStockAmount(int bookItemId, int amount);
        Task<int> GetStockAmountForBookItemByIdAsync(int bookItemId);
        //Task<bool> IsBookItemOnStock(int bookItemId);
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

        public async Task CreateStockAmountAsync(int bookItemId, int? amount)
        {
            if (amount == null)
            {
                amount = 0;
            }

            StockAmount stockAmount = new()
            {
                BookItemID = bookItemId,
                Amount = (int)amount
            };

            await context.StockAmount.AddAsync(stockAmount);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task DeactivateStockAmountAsync(int bookItemId)
        {
            var stockAmount = await context.StockAmount.FirstOrDefaultAsync(x => x.IsActive && x.BookItemID == bookItemId);

            if (stockAmount == null)
            {
                throw new NotFoundException("Nie znaleziono obiektu stock amount dla produktu.");
            }

            stockAmount.IsActive = false;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task UpdateStockAmount(int bookItemId, int amount)
        {
            var stockAmount = await context.StockAmount.FirstOrDefaultAsync(x => x.IsActive && x.BookItemID == bookItemId);

            if (stockAmount == null)
            {
                throw new NotFoundException("Nie znaleziono obiektu stock amount dla produktu.");
            }

            stockAmount.Amount = amount;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task<int> GetStockAmountForBookItemByIdAsync(int bookItemId)
        {
            var stockAmount = await context.StockAmount.FirstOrDefaultAsync(x => x.IsActive && x.BookItemID == bookItemId);

            if (stockAmount == null)
            {
                throw new NotFoundException("Nie znaleziono obiektu stock amount dla produktu.");
            }

            return stockAmount.Amount;
        }

        //public async Task<bool> IsBookItemOnStock(int bookItemId)
        //{
        //    var stockAmount = await context.StockAmount.FirstOrDefaultAsync(x => x.IsActive && x.BookItemID == bookItemId);

        //    if (stockAmount == null)
        //    {
        //        throw new NotFoundException("Nie znaleziono obiektu stock amount dla produktu.");
        //    }

        //    return stockAmount.Amount > 0;
        //}
    }
}
