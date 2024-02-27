using BookStoreAPI.Helpers;
using BookStoreBusinessLogic.BusinessLogic.Discounts;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreDto.Dtos.Orders;
using BookStoreDto.Dtos.Products.BookItems;
using BookStoreDto.Dtos.Wishlists;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Discounts.Discounts
{
    public interface IBookDiscountService
    {
        Task AddNewBookDiscountAsync(int discountId, List<int> bookItemIds);
        Task DeactivateAllBookDiscountsByBookItemAsync(int bookItemId);
        Task DeactivateAllBookDiscountsByDiscountAsync(int discountId);
        Task DeactivateChosenBookDiscountsAsync(int discountId, List<int> bookItemIds);
        Task UpdateBookDiscountAsync(int discountId, List<int> bookItemIds);
        Task<Discount?> GetDiscountForBookItemAsync(int bookItemId);
        Task<IEnumerable<BookDiscount>> GetAllAvailableDiscountsForBookItemIdsAsync(List<int> bookItemIds);
        Task<List<OrderItemsListDto>> ApplyDiscount(List<OrderItemsListDto> cartItems);
        Task<List<BookItemDto>> ApplyDiscount(List<BookItemDto> bookItems);
        Task<List<WishlistItemDto>> ApplyDiscount(List<WishlistItemDto> wishlistBookItems);
    }

    public class BookDiscountService(BookStoreContext context, IDiscountLogic discountLogic) : IBookDiscountService
    {
        public async Task AddNewBookDiscountAsync(int discountId, List<int> bookItemIds)
        {
            var bookItemsToAdd = bookItemIds.Select(bookItemId => new BookDiscount
            {
                BookItemID = bookItemId,
                DiscountID = discountId,
            }).ToList();

            await context.BookDiscount.AddRangeAsync(bookItemsToAdd);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task UpdateBookDiscountAsync(int discountId, List<int> bookItemIds)
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
        public async Task DeactivateChosenBookDiscountsAsync(int discountId, List<int> bookItemIds)
        {
            var discountsToDeactivate = await context.BookDiscount
                .Where(x => x.DiscountID == discountId && bookItemIds.Contains((int)x.BookItemID) && x.IsActive == true)
                .ToListAsync();

            foreach (var discountItem in discountsToDeactivate)
            {
                discountItem.IsActive = false;
                discountItem.ModifiedDate = DateTime.UtcNow;
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
                discount.ModifiedDate = DateTime.UtcNow;
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
        public async Task<List<OrderItemsListDto>> ApplyDiscount(List<OrderItemsListDto> cartItems)
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
                    cartItem.IsDiscounted = true;
                }
            }

            return cartItems;
        }
        public async Task<List<BookItemDto>> ApplyDiscount(List<BookItemDto> bookItems)
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
                    bookItem.DiscountedBruttoPrice = bookItem.Price * (1 - maxDiscount / 100);
                }
            }

            return bookItems;
        }
        public async Task<List<WishlistItemDto>> ApplyDiscount(List<WishlistItemDto> wishlistBookItems)
        {
            var bookItemIds = wishlistBookItems.Select(x => x.Id).ToList();
            var activeDiscounts = await GetAllAvailableDiscountsForBookItemIdsAsync(bookItemIds);

            foreach (var bookItem in wishlistBookItems)
            {
                var applicableDiscounts = activeDiscounts
                    .Where(x => x.BookItemID == bookItem.Id)
                    .Select(x => x.Discount);

                if (applicableDiscounts.Any())
                {
                    var maxDiscount = applicableDiscounts.Max(x => x.PercentOfDiscount);
                    bookItem.DiscountedBruttoPrice = bookItem.BruttoPrice * (1 - maxDiscount / 100);
                }
            }

            return wishlistBookItems;
        }
    }
}
