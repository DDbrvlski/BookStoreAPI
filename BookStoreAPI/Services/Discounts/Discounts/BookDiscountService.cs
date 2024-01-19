using BookStoreAPI.Helpers;
using BookStoreBusinessLogic.BusinessLogic.Discounts;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreViewModels.ViewModels.Orders;
using BookStoreViewModels.ViewModels.Products.BookItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        Task<Discount?> GetDiscountForBookItemAsync(int bookItemId);
        Task<IEnumerable<BookDiscount>> GetAllAvailableDiscountsForBookItemIdsAsync(List<int> bookItemIds);
        Task<List<OrderItemsListViewModel>> ApplyDiscount(List<OrderItemsListViewModel> cartItems);
        Task<List<BookItemViewModel>> ApplyDiscount(List<BookItemViewModel> bookItems);
    }

    public class BookDiscountService(BookStoreContext context, IDiscountLogic discountLogic) : IBookDiscountService
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

        public async Task<Discount?> GetDiscountForBookItemAsync(int bookItemId)
        {
            return await context.BookDiscount
                    .Where(x => x.BookItemID == bookItemId && x.IsActive)
                    .OrderByDescending(x => x.Discount.PercentOfDiscount)
                    .Select(x => x.Discount)
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BookDiscount>> GetAllAvailableDiscountsForBookItemIdsAsync(List<int> bookItemIds)
        {
            return await context.BookDiscount
                .Include(x => x.Discount)
                .Where(x => bookItemIds.Contains((int)x.BookItemID))
                .ToListAsync();
        }
        public async Task<List<OrderItemsListViewModel>> ApplyDiscount(List<OrderItemsListViewModel> cartItems)
        {
            var bookItemIds = cartItems.Select(x => x.BookItemID).ToList();
            var activeDiscounts = await GetAllAvailableDiscountsForBookItemIdsAsync(bookItemIds);

            foreach (var cartItem in cartItems)
            {
                var applicableDiscounts = activeDiscounts
                    .Where(x => x.BookItemID == cartItem.BookItemID)
                    .Select(x => x.Discount);

                if (applicableDiscounts.Any())
                {
                    var maxDiscount = applicableDiscounts.Max(x => x.PercentOfDiscount);
                    cartItem.SingleItemBruttoPrice = discountLogic.CalculateItemPriceWithDiscountCode((decimal)cartItem.SingleItemBruttoPrice, maxDiscount);
                }
            }

            return cartItems;
        }
        public async Task<List<BookItemViewModel>> ApplyDiscount(List<BookItemViewModel> bookItems)
        {
            var bookItemIds = bookItems.Select(x => x.Id).ToList();
            var activeDiscounts = await GetAllAvailableDiscountsForBookItemIdsAsync(bookItemIds);

            foreach (var bookItem in bookItems)
            {
                var applicableDiscounts = activeDiscounts
                    .Where(x => x.BookItemID == bookItem.Id)
                    .Select(x => x.Discount);

                if (applicableDiscounts.Any())
                {
                    var maxDiscount = applicableDiscounts.Max(x => x.PercentOfDiscount);
                    bookItem.DiscountedBruttoPrice = bookItem.Price * (1 + maxDiscount / 100);
                }
            }

            return bookItems;
        }
    }
}
