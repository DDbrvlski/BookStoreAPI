using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Discounts.Discounts;
using BookStoreBusinessLogic.BusinessLogic.Discounts;
using BookStoreData.Data;
using BookStoreData.Models.Customers;
using BookStoreData.Models.Products.BookItems;
using BookStoreViewModels.ViewModels.Orders;
using BookStoreViewModels.ViewModels.Products.BookItems;
using BookStoreViewModels.ViewModels.Products.DiscountCodes;
using BookStoreViewModels.ViewModels.Products.Discounts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BookStoreAPI.Services.Discounts.DiscountCodes
{
    public interface IDiscountCodeService
    {
        Task CreateDiscountCodeAsync(DiscountCodePostCMSViewModel discountCodeModel);
        Task DeactivateDiscountCodeAsync(int discountCodeId);
        Task<IEnumerable<DiscountCodeCMSViewModel>> GetAllDiscountCodesCMSAsync();
        Task<DiscountCodeDetailsCMSViewModel> GetDiscountCodeByIdCMSAsync(int id);
        Task UpdateDiscountCodeAsync(int discountCodeId, DiscountCodePostCMSViewModel discountCodeModel);
        Task<List<OrderItemsListViewModel>> ApplyDiscountCodeToCartItemsAsync(List<OrderItemsListViewModel> cartItems, int discountCodeId);
        Task<DiscountCode> CheckIfDiscountCodeIsValidAsync(string discountName);
    }

    public class DiscountCodeService(BookStoreContext context, IDiscountLogic discountLogic) : IDiscountCodeService
    {
        public async Task<DiscountCodeDetailsCMSViewModel> GetDiscountCodeByIdCMSAsync(int id)
        {
            return await context.DiscountCode
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new DiscountCodeDetailsCMSViewModel()
                {
                    Id = x.Id,
                    IsAvailable = DateTime.Today >= x.StartingDate && DateTime.Today <= x.ExpiryDate.AddDays(1),
                    Code = x.Code,
                    Description = x.Description,
                    ExpiryDate = x.ExpiryDate,
                    PercentOfDiscount = x.PercentOfDiscount,
                    StartingDate = x.StartingDate,
                }).FirstAsync();
        }

        public async Task<IEnumerable<DiscountCodeCMSViewModel>> GetAllDiscountCodesCMSAsync()
        {
            return await context.DiscountCode
                .Where(x => x.IsActive == true)
                .Select(x => new DiscountCodeCMSViewModel
                {
                    Id = x.Id,
                    IsAvailable = DateTime.Today >= x.StartingDate && DateTime.Today <= x.ExpiryDate.AddDays(1),
                    Description = x.Description,
                    PercentOfDiscount = x.PercentOfDiscount,
                    Code = x.Code
                })
                .ToListAsync();
        }

        public async Task CreateDiscountCodeAsync(DiscountCodePostCMSViewModel discountCodeModel)
        {
            DiscountCode discountCode = new();
            discountCode.CopyProperties(discountCodeModel);
            await context.DiscountCode.AddAsync(discountCode);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task UpdateDiscountCodeAsync(int discountCodeId, DiscountCodePostCMSViewModel discountCodeModel)
        {
            var discountCode = await context.DiscountCode.FirstOrDefaultAsync(x => x.IsActive && x.Id == discountCodeId);

            if (discountCode == null)
            {
                throw new BadRequestException("Nie znaleziono elementu discountCode do aktualizacji.");
            }

            discountCode.CopyProperties(discountCodeModel);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task DeactivateDiscountCodeAsync(int discountCodeId)
        {
            var discountCode = await context.DiscountCode.FirstOrDefaultAsync(x => x.IsActive && x.Id == discountCodeId);

            if (discountCode == null)
            {
                throw new BadRequestException("Nie znaleziono elementu discountCode do aktualizacji.");
            }

            discountCode.IsActive = false;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task<DiscountCode?> CheckIfDiscountCodeIsValidAsync(int discountCodeId)
        {
            return await GetDiscountCodeByDataAsync(x => x.Id == discountCodeId);
        }
        public async Task<DiscountCode> CheckIfDiscountCodeIsValidAsync(string discountCode)
        {
            var discount = await GetDiscountCodeByDataAsync(x => x.Code == discountCode);

            if (discount == null)
            {
                throw new BadRequestException("Podany kod jest niepoprawny.");
            }

            return discount;
        }
        
        public async Task<DiscountCode?> GetDiscountCodeByDataAsync(Expression<Func<DiscountCode, bool>> discountCodeFunction)
        {
            return await context.DiscountCode.Where(x => x.IsActive).FirstOrDefaultAsync(discountCodeFunction);
        }

        public async Task<List<OrderItemsListViewModel>> ApplyDiscountCodeToCartItemsAsync(List<OrderItemsListViewModel> cartItems, int discountCodeId)
        {
            var discount = await CheckIfDiscountCodeIsValidAsync(discountCodeId);
            foreach (var cartItem in cartItems)
            {
                cartItem.SingleItemBruttoPrice = discountLogic.CalculateItemPriceWithDiscountCode((decimal)cartItem.SingleItemBruttoPrice, discount.PercentOfDiscount);
            }

            return cartItems;
        }
    }
}
