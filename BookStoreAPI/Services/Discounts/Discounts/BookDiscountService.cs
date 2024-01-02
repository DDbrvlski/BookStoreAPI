using BookStoreAPI.Helpers;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStoreAPI.Services.Discounts.Discounts
{
    public interface IBookDiscountService
    {
        Task AddNewBookDiscountAsync(int discountId, List<int?> bookItemIds);
        Task DeactivateAllBookDiscountsByBookItemAsync(int bookItemId);
        Task DeactivateAllBookDiscountsByDiscountAsync(int discountId);
        Task DeactivateChosenBookDiscountsAsync(int discountId, List<int?> bookItemIds);
        Task UpdateBookDiscountAsync(int discountId, List<int?> bookItemIds);
    }

    public class BookDiscountService(BookStoreContext context) : IBookDiscountService
    {
        public async Task AddNewBookDiscountAsync(int discountId, List<int?> bookItemIds)
        {
            var bookItemsToAdd = bookItemIds.Select(bookItemId => new BookDiscount
            {
                BookItemID = bookItemId,
                DiscountID = discountId,
            }).ToList();

            context.BookDiscount.AddRange(bookItemsToAdd);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task UpdateBookDiscountAsync(int discountId, List<int?> bookItemIds)
        {
            var existingBookDiscounts = await context.BookDiscount
                .Where(x => x.DiscountID == discountId && x.IsActive == true)
                .Select(x => x.BookItemID)
                .ToListAsync();

            var bookDiscountsToDeactivate = existingBookDiscounts.Except(bookItemIds).ToList();
            var bookDiscountsToAdd = bookItemIds.Except(existingBookDiscounts).ToList();

            if (bookDiscountsToDeactivate.Any())
            {
                await DeactivateChosenBookDiscountsAsync(discountId, bookDiscountsToDeactivate);
            }

            if (bookDiscountsToAdd.Any())
            {
                await AddNewBookDiscountAsync(discountId, bookDiscountsToAdd);
            }
        }
        public async Task DeactivateChosenBookDiscountsAsync(int discountId, List<int?> bookItemIds)
        {
            var discountsToDeactivate = await context.BookDiscount
                .Where(x => x.DiscountID == discountId && bookItemIds.Contains((int)x.BookItemID) && x.IsActive == true)
                .ToListAsync();

            foreach (var discountItem in discountsToDeactivate)
            {
                discountItem.IsActive = false;
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateAllBookDiscountsByDiscountAsync(int discountId)
        {
            var bookDiscounts = await context.BookDiscount.Where(x => x.IsActive && x.DiscountID == discountId).ToListAsync();

            foreach (var discount in bookDiscounts)
            {
                discount.IsActive = false;
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateAllBookDiscountsByBookItemAsync(int bookItemId)
        {
            var bookDiscounts = await context.BookDiscount.Where(x => x.IsActive && x.BookItemID == bookItemId).ToListAsync();

            foreach (var discount in bookDiscounts)
            {
                discount.IsActive = false;
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
