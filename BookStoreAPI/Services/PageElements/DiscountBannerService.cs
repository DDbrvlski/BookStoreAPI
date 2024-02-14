using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Media;
using BookStoreData.Data;
using BookStoreData.Models.PageContent;
using BookStoreDto.Dtos.PageContent.Banners;
using BookStoreDto.Dtos.PageContent.DiscountBanners;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.PageElements
{
    public interface IDiscountBannerService
    {
        Task CreateDiscountBannerAsync(DiscountBannerDto discountBannerModel);
        Task DeactivateDiscountBannerAsync(int discountBannerId);
        Task EditDiscountBannerAsync(int discountBannerId, DiscountBannerDto discountBannerModel);
        Task<IEnumerable<DiscountBannerDto>> GetAllDiscountBannersAsync();
        Task<DiscountBannerDto> GetDiscountBannerByIdAsync(int bannerId);
    }

    public class DiscountBannerService(BookStoreContext context, IImageService imageService) : IDiscountBannerService
    {
        public async Task<IEnumerable<DiscountBannerDto>> GetAllDiscountBannersAsync()
        {
            return await context.DiscountsBanner
                .Include(x => x.Image)
                .Where(x => x.IsActive == true)
                .Select(x => new DiscountBannerDto
                {
                    Id = x.Id,
                    ButtonTitle = x.ButtonTitle,
                    Header = x.Header,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).ToListAsync();
        }
        public async Task<DiscountBannerDto> GetDiscountBannerByIdAsync(int bannerId)
        {
            return await context.DiscountsBanner
                .Include(x => x.Image)
                .Where(x => x.IsActive == true && x.Id == bannerId)
                .Select(x => new DiscountBannerDto
                {
                    Id = x.Id,
                    ButtonTitle = x.ButtonTitle,
                    Header = x.Header,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).FirstAsync();
        }
        public async Task CreateDiscountBannerAsync(DiscountBannerDto discountBannerModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var newImage = await imageService.AddNewImageAndReturnAsync(discountBannerModel.ImageTitle, discountBannerModel.ImageURL);

                    DiscountsBanner newDiscountBanner = new();
                    newDiscountBanner.CopyProperties(discountBannerModel);
                    newDiscountBanner.ImageID = newImage.Id;

                    await context.DiscountsBanner.AddAsync(newDiscountBanner);
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }            
        }
        public async Task EditDiscountBannerAsync(int discountBannerId, DiscountBannerDto discountBannerModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var discountBanner = await context.DiscountsBanner.Include(x => x.Image).FirstAsync(x => x.IsActive && x.Id == discountBannerId);
                    discountBanner.CopyProperties(discountBannerModel);

                    if (discountBannerModel.ImageURL != discountBanner.Image.ImageURL)
                    {
                        await imageService.DeactivateSingleImageByIdAsync(discountBanner.ImageID);
                        var newImage = await imageService.AddNewImageAndReturnAsync(discountBannerModel.ImageURL, discountBannerModel.ImageURL);

                        discountBanner.ImageID = newImage.Id;
                    }

                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
            
        }
        public async Task DeactivateDiscountBannerAsync(int discountBannerId)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var discountBanner = await context.DiscountsBanner.FirstAsync(x => x.IsActive && x.Id == discountBannerId);
                    discountBanner.IsActive = false;

                    await imageService.DeactivateSingleImageByIdAsync(discountBanner.ImageID);

                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
            
        }
    }
}
