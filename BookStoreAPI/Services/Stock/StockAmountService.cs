using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Availability;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreViewModels.ViewModels.Availability;
using BookStoreViewModels.ViewModels.Products.StockAmount;
using BookStoreViewModels.ViewModels.Stock;
using BookStoreViewModels.ViewModels.Supply;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStoreAPI.Services.Stock
{
    public interface IStockAmountService
    {
        Task CreateStockAmountAsync(int bookItemId, int? amount);
        Task DeactivateStockAmountAsync(int bookItemId);
        Task<ActionResult<IEnumerable<StockAmountViewModel>>> GetAllStockAmountAsync();
        Task<ActionResult<StockAmountViewModel?>> GetStockAmountByIdAsync(int id);
        Task UpdateStockAmountAsync(int bookItemId, int amount);
        Task<int> GetStockAmountForBookItemByIdAsync(int bookItemId);
        Task UpdateStockAmountAsync(List<BookItemStockAmountUpdateViewModel> bookItems);
        //Task AddNewReservationInStockAsync(int bookItemId);
        //Task CancelReservationInStockAsync(int bookItemId);
        //Task<bool> IsBookItemOnStock(int bookItemId);
    }

    public class StockAmountService(BookStoreContext context, IAvailabilityService availabilityService) : IStockAmountService
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

            await availabilityService.UpdateBookItemAvailabilityAsync(bookItemId, stockAmount.Amount);
        }

        public async Task DeactivateStockAmountAsync(int bookItemId)
        {
            var stockAmount = await context.StockAmount.FirstOrDefaultAsync(x => x.IsActive && x.BookItemID == bookItemId);

            if (stockAmount == null)
            {
                throw new NotFoundException("Nie znaleziono obiektu stock amount dla produktu.");
            }

            await availabilityService.DisableAvailabilityForBookItemAsync(bookItemId);
            stockAmount.IsActive = false;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task UpdateStockAmountAsync(int bookItemId, int amount)
        {
            var stockAmount = await context.StockAmount.FirstOrDefaultAsync(x => x.IsActive && x.BookItemID == bookItemId);

            if (stockAmount == null)
            {
                throw new NotFoundException("Nie znaleziono obiektu stock amount dla produktu.");
            }

            stockAmount.Amount = amount;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            await availabilityService.UpdateBookItemAvailabilityAsync(bookItemId, stockAmount.Amount);
        }
        public async Task UpdateStockAmountAsync(List<BookItemStockAmountUpdateViewModel> bookItems)
        {
            var bookItemIds = bookItems.Select(y => y.BookItemId).ToList();

            var bookItemsToUpdate = await context.StockAmount
                .Where(x => x.IsActive && bookItemIds.Contains((int)x.BookItemID))
                .ToListAsync();

            foreach (var bookItem in bookItemsToUpdate)
            {
                var updateData = bookItems.FirstOrDefault(y => y.BookItemId == bookItem.BookItemID);

                if (updateData != null)
                {
                    bookItem.Amount += updateData.Quantity;
                    if (bookItem.Amount < 0)
                    {
                        throw new BadRequestException($"Wystąpił błąd z aktualizacją stanu magazynowego dla książki {bookItem.BookItemID}, ilość nie może zejść poniżej 0.");
                        //bookItem.Amount = 0;
                    }
                }
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            List<AvailabilityBookItemsCheckViewModel> stockBookItems = new();
            foreach (var bookItem in bookItemsToUpdate)
            {
                stockBookItems.Add(new AvailabilityBookItemsCheckViewModel() { BookItemId = (int)bookItem.BookItemID, StockAmount = bookItem.Amount });
            }

            await availabilityService.UpdateBookItemsAvailabilityAsync(stockBookItems);
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

        //public async Task AddNewReservationInStockAsync(int bookItemId)
        //{
        //    var stock = await context.StockAmount.Where(x => x.IsActive && x.BookItemID == bookItemId).FirstOrDefaultAsync();
        //    if (stock == null)
        //    {
        //        throw new NotFoundException("Nie znaleziono książki w magazynie");
        //    }

        //    stock.ReservedAmount++;
        //    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        //    await availabilityService.UpdateBookItemAvailabilityAsync(bookItemId, stock.Amount);
        //}
        //public async Task CancelReservationInStockAsync(int bookItemId)
        //{
        //    var stock = await context.StockAmount.Where(x => x.IsActive && x.BookItemID == bookItemId).FirstOrDefaultAsync();
        //    if (stock == null)
        //    {
        //        throw new NotFoundException("Nie znaleziono książki w magazynie");
        //    }

        //    stock.ReservedAmount--;
        //    if(stock.ReservedAmount < 0)
        //    {
        //        throw new BadRequestException("Wystąpił błąd z anulowaniem rezerwacji.");
        //    }

        //    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        //    await availabilityService.UpdateBookItemAvailabilityAsync(bookItemId, stock.Amount, stock.ReservedAmount);
        //}

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
