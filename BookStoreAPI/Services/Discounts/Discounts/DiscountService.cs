using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreViewModels.ViewModels.Products.BookItems;
using BookStoreViewModels.ViewModels.Products.Discounts;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Discounts.Discounts
{
    public interface IDiscountService
    {
        Task CreateDiscountAsync(DiscountCMSPostViewModel discountModel);
        Task DeactivateDiscountAsync(int discountId);
        Task<IEnumerable<DiscountCMSViewModel>> GetAllDiscountsCMSAsync();
        Task<DiscountDetailsCMSViewModel> GetDiscountByIdCMSAsync(int id);
        Task UpdateDiscountAsync(int discountId, DiscountCMSPostViewModel discountModel);
    }

    public class DiscountService(BookStoreContext context, IBookDiscountService bookDiscountService) : IDiscountService
    {
        public async Task<DiscountDetailsCMSViewModel> GetDiscountByIdCMSAsync(int id)
        {
            return await context.Discount
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new DiscountDetailsCMSViewModel()
                {
                    Id = x.Id,
                    IsAvailable = DateTime.Today >= x.StartingDate && DateTime.Today <= x.ExpiryDate.AddDays(1),
                    Description = x.Description,
                    ExpiryDate = x.ExpiryDate,
                    StartingDate = x.StartingDate,
                    PercentOfDiscount = x.PercentOfDiscount,
                    Title = x.Title,
                    ListOfBookItems = x.BookDiscounts
                        .Where(x => x.IsActive == true)
                        .Select(x => new BookItemCMSViewModel
                        {
                            Id = x.BookItem.Id,
                            BookID = x.BookItem.BookID,
                            BookTitle = x.BookItem.Book.Title,
                            ISBN = x.BookItem.ISBN,
                            FormName = x.BookItem.Form.Name,
                            NettoPrice = x.BookItem.NettoPrice,
                        }).ToList()
                }).FirstAsync();
        }

        public async Task<IEnumerable<DiscountCMSViewModel>> GetAllDiscountsCMSAsync()
        {
            return await context.Discount
                .Where(x => x.IsActive == true)
                .Select(x => new DiscountCMSViewModel
                {
                    Id = x.Id,
                    IsAvailable = DateTime.Today >= x.StartingDate && DateTime.Today <= x.ExpiryDate.AddDays(1),
                    Description = x.Description,
                    PercentOfDiscount = x.PercentOfDiscount,
                    Title = x.Title,
                })
                .ToListAsync();
        }

        public async Task CreateDiscountAsync(DiscountCMSPostViewModel discountModel)
        {
            Discount discount = new();
            discount.CopyProperties(discountModel);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            await bookDiscountService.AddNewBookDiscountAsync(discount.Id, discountModel.ListOfBookItems.Select(x => x.Id).ToList());
        }

        public async Task UpdateDiscountAsync(int discountId, DiscountCMSPostViewModel discountModel)
        {
            var discount = await context.Discount.FirstOrDefaultAsync(x => x.IsActive && x.Id == discountId);

            if (discount == null)
            {
                throw new BadRequestException("Nie znaleziono elementu discount do aktualizacji.");
            }

            discount.CopyProperties(discountModel);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            await bookDiscountService.UpdateBookDiscountAsync(discountId, discountModel.ListOfBookItems.Select(x => x.Id).ToList());
        }

        public async Task DeactivateDiscountAsync(int discountId)
        {
            var discount = await context.Discount.FirstOrDefaultAsync(x => x.IsActive && x.Id == discountId);

            if (discount == null)
            {
                throw new BadRequestException("Nie znaleziono elementu discount do deaktywacji.");
            }

            discount.IsActive = false;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            await bookDiscountService.DeactivateAllBookDiscountsByDiscountAsync(discountId);
        }
    }
}
