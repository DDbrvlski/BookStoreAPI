using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Media;
using BookStoreData.Data;
using BookStoreData.Models.PageContent;
using BookStoreViewModels.ViewModels.PageContent.Banners;
using BookStoreViewModels.ViewModels.PageContent.DiscountBanners;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.PageElements
{
    public interface IDiscountBannerService
    {
        Task CreateDiscountBannerAsync(DiscountBannerViewModel discountBannerModel);
        Task DeactivateDiscountBannerAsync(int discountBannerId);
        Task EditDiscountBannerAsync(int discountBannerId, DiscountBannerViewModel discountBannerModel);
        Task<IEnumerable<DiscountBannerViewModel>> GetAllDiscountBannersAsync();
        Task<DiscountBannerViewModel> GetDiscountBannerByIdAsync(int bannerId);
    }

    public class DiscountBannerService(BookStoreContext context, IImageService imageService) : IDiscountBannerService
    {
        public async Task<IEnumerable<DiscountBannerViewModel>> GetAllDiscountBannersAsync()
        {
            return await context.DiscountsBanner
                .Include(x => x.Image)
                .Where(x => x.IsActive == true)
                .Select(x => new DiscountBannerViewModel
                {
                    Id = x.Id,
                    ButtonTitle = x.ButtonTitle,
                    Header = x.Header,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).ToListAsync();
        }
        public async Task<DiscountBannerViewModel> GetDiscountBannerByIdAsync(int bannerId)
        {
            return await context.DiscountsBanner
                .Include(x => x.Image)
                .Where(x => x.IsActive == true && x.Id == bannerId)
                .Select(x => new DiscountBannerViewModel
                {
                    Id = x.Id,
                    ButtonTitle = x.ButtonTitle,
                    Header = x.Header,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).FirstAsync();
        }
        public async Task CreateDiscountBannerAsync(DiscountBannerViewModel discountBannerModel)
        {
            var newImage = await imageService.AddNewImageAndReturnAsync(discountBannerModel.ImageTitle, discountBannerModel.ImageURL);

            DiscountsBanner newDiscountBanner = new();
            newDiscountBanner.CopyProperties(discountBannerModel);
            newDiscountBanner.ImageID = newImage.Id;

            await context.DiscountsBanner.AddAsync(newDiscountBanner);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task EditDiscountBannerAsync(int discountBannerId, DiscountBannerViewModel discountBannerModel)
        {
            var discountBanner = await context.DiscountsBanner.FirstAsync(x => x.IsActive && x.Id == discountBannerId);
            discountBanner.CopyProperties(discountBannerModel);

            if (discountBannerModel.ImageURL != discountBanner.Image.ImageURL)
            {
                await imageService.DeactivateSingleImageByIdAsync(discountBanner.ImageID);
                var newImage = await imageService.AddNewImageAndReturnAsync(discountBannerModel.ImageURL, discountBannerModel.ImageURL);

                discountBanner.ImageID = newImage.Id;
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateDiscountBannerAsync(int discountBannerId)
        {
            var discountBanner = await context.DiscountsBanner.FirstAsync(x => x.IsActive && x.Id == discountBannerId);
            discountBanner.IsActive = false;

            await imageService.DeactivateSingleImageByIdAsync(discountBanner.ImageID);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
