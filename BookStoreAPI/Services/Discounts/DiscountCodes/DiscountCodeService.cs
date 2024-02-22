using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Discounts.Discounts;
using BookStoreBusinessLogic.BusinessLogic.Discounts;
using BookStoreData.Data;
using BookStoreData.Models.Customers;
using BookStoreData.Models.Products.BookItems;
using BookStoreDto.Dtos.Orders;
using BookStoreDto.Dtos.Products.BookItems;
using BookStoreDto.Dtos.Products.DiscountCodes;
using BookStoreDto.Dtos.Products.Discounts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BookStoreAPI.Services.Discounts.DiscountCodes
{
    public interface IDiscountCodeService
    {
        Task CreateDiscountCodeAsync(DiscountCodePostCMSDto discountCodeModel);
        Task DeactivateDiscountCodeAsync(int discountCodeId);
        Task<IEnumerable<DiscountCodeCMSDto>> GetAllDiscountCodesCMSAsync();
        Task<DiscountCodeDetailsCMSDto> GetDiscountCodeByIdCMSAsync(int id);
        Task UpdateDiscountCodeAsync(int discountCodeId, DiscountCodePostCMSDto discountCodeModel);
        Task<List<OrderItemsListDto>> ApplyDiscountCodeToCartItemsAsync(List<OrderItemsListDto> cartItems, int discountCodeId);
        Task<DiscountCode> CheckIfDiscountCodeIsValidAsync(string discountName);
    }

    public class DiscountCodeService(BookStoreContext context, IDiscountLogic discountLogic) : IDiscountCodeService
    {
        public async Task<DiscountCodeDetailsCMSDto> GetDiscountCodeByIdCMSAsync(int id)
        {
            var code = await context.DiscountCode
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new DiscountCodeDetailsCMSDto()
                {
                    Id = x.Id,
                    IsAvailable = false,
                    Code = x.Code,
                    Description = x.Description,
                    ExpiryDate = x.ExpiryDate,
                    PercentOfDiscount = x.PercentOfDiscount,
                    StartingDate = x.StartingDate,
                }).FirstAsync();

            var todaysDate = DateTime.Now.Date;
            code.IsAvailable = (todaysDate >= code.StartingDate.Date && todaysDate <= code.ExpiryDate.AddDays(1).Date);

            return code;
        }

        public async Task<IEnumerable<DiscountCodeCMSDto>> GetAllDiscountCodesCMSAsync()
        {
            var todaysDate = DateTime.Now.Date;
            var codes = await context.DiscountCode
                .Where(x => x.IsActive == true)
                .Select(x => new DiscountCodeCMSDto
                {
                    Id = x.Id,
                    IsAvailable = false,
                    Description = x.Description,
                    PercentOfDiscount = x.PercentOfDiscount,
                    StartingDate = x.StartingDate,
                    ExpiryDate = x.ExpiryDate,
                    Code = x.Code
                })
                .ToListAsync();

            codes.ForEach(x => x.IsAvailable = todaysDate >= x.StartingDate.Date && todaysDate <= x.ExpiryDate.AddDays(1).Date);

            return codes;
        }

        public async Task CreateDiscountCodeAsync(DiscountCodePostCMSDto discountCodeModel)
        {
            DiscountCode discountCode = new();
            discountCode.CopyProperties(discountCodeModel);
            await context.DiscountCode.AddAsync(discountCode);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task UpdateDiscountCodeAsync(int discountCodeId, DiscountCodePostCMSDto discountCodeModel)
        {
            var discountCode = await context.DiscountCode.FirstOrDefaultAsync(x => x.IsActive && x.Id == discountCodeId);

            if (discountCode == null)
            {
                throw new BadRequestException("Nie znaleziono elementu discountCode do aktualizacji.");
            }

            discountCode.ModifiedDate = DateTime.UtcNow;
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

            discountCode.ModifiedDate = DateTime.UtcNow;
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

            var todaysDate = DateTime.UtcNow.Date;

            if (discount.StartingDate.Date > todaysDate || discount.ExpiryDate.Date < todaysDate)
            {
                throw new BadRequestException("Podany kod nie jest ważny.");
            }

            return discount;
        }
        
        public async Task<DiscountCode?> GetDiscountCodeByDataAsync(Expression<Func<DiscountCode, bool>> discountCodeFunction)
        {
            return await context.DiscountCode.Where(x => x.IsActive).FirstOrDefaultAsync(discountCodeFunction);
        }

        public async Task<List<OrderItemsListDto>> ApplyDiscountCodeToCartItemsAsync(List<OrderItemsListDto> cartItems, int discountCodeId)
        {
            var discount = await CheckIfDiscountCodeIsValidAsync(discountCodeId);
            foreach (var cartItem in cartItems)
            {
                cartItem.SingleItemBruttoPrice = discountLogic.CalculateItemPriceWithDiscountCode((decimal)cartItem.SingleItemBruttoPrice, discount.PercentOfDiscount);
                cartItem.IsDiscounted = true;
            }

            return cartItems;
        }
    }
}
