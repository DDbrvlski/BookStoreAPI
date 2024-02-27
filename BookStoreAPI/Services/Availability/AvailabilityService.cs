using BookStoreAPI.Enums;
using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
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
            if (bookItem.FormID == (int)BookFormEnum.Book)
            {
                if (stockAmount <= 0)
                {
                    bookItem.AvailabilityID = (int)AvailabilityEnum.Niedostepna;
                }
                else if (stockAmount > 0)
                {
                    bookItem.AvailabilityID = (int)AvailabilityEnum.Dostepna;
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
            bookItem.ModifiedDate = DateTime.UtcNow;
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
                        bookItem.AvailabilityID = (int)AvailabilityEnum.Niedostepna;
                    }
                    else if (bookItemAmount > 0)
                    {
                        bookItem.AvailabilityID = (int)AvailabilityEnum.Dostepna;
                    }
                    else
                    {
                        throw new BadRequestException("Wystąpił błąd podczas aktualizowania stanu magazynowego produktu.");
                    }
                }
                else
                {
                    bookItem.AvailabilityID = (int)AvailabilityEnum.Dostepna;
                }
                bookItem.ModifiedDate = DateTime.UtcNow;
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

            bookItem.AvailabilityID = (int)AvailabilityEnum.Niedostepna;
            bookItem.ModifiedDate = DateTime.UtcNow;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
