using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreDto.Dtos.Products.BookItems;
using BookStoreDto.Dtos.Products.Discounts;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Discounts.Discounts
{
    public interface IDiscountService
    {
        Task CreateDiscountAsync(DiscountCMSPostDto discountModel);
        Task DeactivateDiscountAsync(int discountId);
        Task<IEnumerable<DiscountCMSDto>> GetAllDiscountsCMSAsync();
        Task<DiscountDetailsCMSDto> GetDiscountByIdCMSAsync(int id);
        Task UpdateDiscountAsync(int discountId, DiscountCMSPostDto discountModel);
    }

    public class DiscountService(BookStoreContext context, IBookDiscountService bookDiscountService) : IDiscountService
    {
        public async Task<DiscountDetailsCMSDto> GetDiscountByIdCMSAsync(int id)
        {
            var currentDate = DateTime.Now;
            return await context.Discount
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new DiscountDetailsCMSDto()
                {
                    Id = x.Id,
                    IsAvailable = currentDate >= x.StartingDate && currentDate <= x.ExpiryDate.AddDays(1),
                    Description = x.Description,
                    ExpiryDate = x.ExpiryDate,
                    StartingDate = x.StartingDate,
                    PercentOfDiscount = x.PercentOfDiscount,
                    Title = x.Title,
                    ListOfBookItems = x.BookDiscounts
                        .Where(x => x.IsActive == true)
                        .Select(x => new BookItemCMSDto
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

        public async Task<IEnumerable<DiscountCMSDto>> GetAllDiscountsCMSAsync()
        {
            var currentDate = DateTime.Now;
            return await context.Discount
                .Where(x => x.IsActive == true)
                .Select(x => new DiscountCMSDto
                {
                    Id = x.Id,
                    IsAvailable = currentDate >= x.StartingDate && currentDate <= x.ExpiryDate.AddDays(1),
                    Description = x.Description,
                    PercentOfDiscount = x.PercentOfDiscount,
                    Title = x.Title,
                })
                .ToListAsync();
        }

        public async Task CreateDiscountAsync(DiscountCMSPostDto discountModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Discount discount = new();
                    discount.CopyProperties(discountModel);
                    await context.Discount.AddAsync(discount);

                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    await bookDiscountService.AddNewBookDiscountAsync(discount.Id, discountModel.ListOfBookItems.Select(x => x.Id).ToList());
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }            
        }

        public async Task UpdateDiscountAsync(int discountId, DiscountCMSPostDto discountModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var discount = await context.Discount.FirstOrDefaultAsync(x => x.IsActive && x.Id == discountId);

                    if (discount == null)
                    {
                        throw new BadRequestException("Nie znaleziono elementu discount do aktualizacji.");
                    }

                    discount.CopyProperties(discountModel);
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    await bookDiscountService.UpdateBookDiscountAsync(discountId, discountModel.ListOfBookItems.Select(x => x.Id).ToList());
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }            
        }

        public async Task DeactivateDiscountAsync(int discountId)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var discount = await context.Discount.FirstOrDefaultAsync(x => x.IsActive && x.Id == discountId);

                    if (discount == null)
                    {
                        throw new BadRequestException("Nie znaleziono elementu discount do deaktywacji.");
                    }

                    discount.IsActive = false;
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    await bookDiscountService.DeactivateAllBookDiscountsByDiscountAsync(discountId);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }            
        }

    }
}
