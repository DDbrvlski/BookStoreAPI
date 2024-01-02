using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Media;
using BookStoreData.Data;
using BookStoreData.Models.PageContent;
using BookStoreViewModels.ViewModels.PageContent.Banners;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.PageElements
{
    public interface IBannerService
    {
        Task CreateBannerAsync(BanerViewModel banerModel);
        Task DeactivateBannerAsync(int bannerId);
        Task EditBannerAsync(int bannerId, BanerViewModel banerModel);
        Task<IEnumerable<BanerViewModel>> GetAllBannersAsync();
        Task<BanerViewModel> GetBannerByIdAsync(int bannerId);
    }

    public class BannerService(BookStoreContext context, IImageService imageService) : IBannerService
    {
        public async Task<IEnumerable<BanerViewModel>> GetAllBannersAsync()
        {
            return await context.Banner
                .Where(x => x.IsActive == true)
                .Select(x => new BanerViewModel
                {
                    Id = x.Id,
                    Path = x.Path,
                    Title = x.Title,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).ToListAsync();
        }
        public async Task<BanerViewModel> GetBannerByIdAsync(int bannerId)
        {
            return await context.Banner
                .Where(x => x.IsActive == true && x.Id == bannerId)
                .Select(x => new BanerViewModel
                {
                    Id = x.Id,
                    Path = x.Path,
                    Title = x.Title,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).FirstAsync();
        }
        public async Task CreateBannerAsync(BanerViewModel banerModel)
        {
            var newImage = await imageService.AddNewImageAndReturnAsync(banerModel.ImageTitle, banerModel.ImageURL);

            Banner newBanner = new()
            {
                Path = banerModel.Path,
                Title = banerModel.Title,
                ImageID = newImage.Id,
            };

            await context.Banner.AddAsync(newBanner);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task EditBannerAsync(int bannerId, BanerViewModel banerModel)
        {
            var banner = await context.Banner.FirstAsync(x => x.IsActive && x.Id == bannerId);
            banner.CopyProperties(banerModel);

            if (banerModel.ImageURL != banner.Image.ImageURL)
            {
                await imageService.DeactivateSingleImageByIdAsync(banner.ImageID);
                var newImage = await imageService.AddNewImageAndReturnAsync(banerModel.ImageURL, banerModel.ImageURL);

                banner.ImageID = newImage.Id;
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateBannerAsync(int bannerId)
        {
            var banner = await context.Banner.FirstAsync(x => x.IsActive && x.Id == bannerId);
            banner.IsActive = false;

            await imageService.DeactivateSingleImageByIdAsync(banner.ImageID);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
