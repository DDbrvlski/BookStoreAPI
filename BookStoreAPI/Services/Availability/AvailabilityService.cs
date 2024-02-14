using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreDto.Dtos.Availability;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Availability
{
    public interface IAvailabilityService
    {
        Task UpdateBookItemAvailabilityAsync(int bookItemId, int stockAmount = 0);
        Task UpdateBookItemsAvailabilityAsync(List<AvailabilityBookItemsCheckDto> stockBookItems);
        Task DisableAvailabilityForBookItemAsync(int bookItemId);
    }

    public class AvailabilityService(BookStoreContext context) : IAvailabilityService
    {
        public async Task UpdateBookItemAvailabilityAsync(int bookItemId, int stockAmount = 0)
        {
            var bookItem = await context.BookItem.Where(x => x.IsActive && x.Id == bookItemId).FirstOrDefaultAsync();
            if (bookItem == null)
            {
                throw new NotFoundException("Wystąpił błąd podczas pobierania książki do aktualizacji dostępności");
            }
            if (bookItem.FormID == 1)
            {
                if (stockAmount <= 0)
                {
                    bookItem.AvailabilityID = 2;
                }
                else if (stockAmount > 0)
                {
                    bookItem.AvailabilityID = 1;
                }
                else
                {
                    throw new BadRequestException("Wystąpił błąd podczas aktualizowania stanu magazynowego produktu.");
                }
            }
            else
            {
                bookItem.AvailabilityID = 1;
            }
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task UpdateBookItemsAvailabilityAsync(List<AvailabilityBookItemsCheckDto> stockBookItems)
        {
            var bookItemIds = stockBookItems.Select(x => x.BookItemId).ToList();
            var bookItems = await context.BookItem.Where(x => x.IsActive && bookItemIds.Contains(x.Id)).ToListAsync();
            foreach (var bookItem in bookItems)
            {
                var bookItemAmount = stockBookItems.First(x => x.BookItemId == bookItem.Id).StockAmount;
                if (bookItem.FormID == 1)
                {
                    if (bookItemAmount <= 0)
                    {
                        bookItem.AvailabilityID = 2;
                    }
                    else if (bookItemAmount > 0)
                    {
                        bookItem.AvailabilityID = 1;
                    }
                    else
                    {
                        throw new BadRequestException("Wystąpił błąd podczas aktualizowania stanu magazynowego produktu.");
                    }
                }
                else
                {
                    bookItem.AvailabilityID = 1;
                }
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DisableAvailabilityForBookItemAsync(int bookItemId)
        {
            var bookItem = await context.BookItem.Where(x => x.Id == bookItemId).FirstOrDefaultAsync();

            if(bookItem == null)
            {
                throw new NotFoundException("Wystąpił błąd podczas edycji dostępności dla książki.");
            }

            bookItem.AvailabilityID = 2;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
