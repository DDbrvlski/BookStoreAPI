using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Media;
using BookStoreData.Data;
using BookStoreData.Models.PageContent;
using BookStoreDto.Dtos.PageContent.Banners;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.PageElements
{
    public interface IBannerService
    {
        Task CreateBannerAsync(BanerDto banerModel);
        Task DeactivateBannerAsync(int bannerId);
        Task EditBannerAsync(int bannerId, BanerDto banerModel);
        Task<IEnumerable<BanerDto>> GetAllBannersAsync();
        Task<BanerDto> GetBannerByIdAsync(int bannerId);
    }

    public class BannerService(BookStoreContext context, IImageService imageService) : IBannerService
    {
        public async Task<IEnumerable<BanerDto>> GetAllBannersAsync()
        {
            return await context.Banner
                .Where(x => x.IsActive == true)
                .Select(x => new BanerDto
                {
                    Id = x.Id,
                    Path = x.Path,
                    Title = x.Title,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).ToListAsync();
        }
        public async Task<BanerDto> GetBannerByIdAsync(int bannerId)
        {
            return await context.Banner
                .Where(x => x.IsActive == true && x.Id == bannerId)
                .Select(x => new BanerDto
                {
                    Id = x.Id,
                    Path = x.Path,
                    Title = x.Title,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).FirstAsync();
        }
        public async Task CreateBannerAsync(BanerDto banerModel)
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
        public async Task EditBannerAsync(int bannerId, BanerDto banerModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var banner = await context.Banner.Include(x => x.Image).FirstAsync(x => x.IsActive && x.Id == bannerId);
                    banner.CopyProperties(banerModel);

                    if (banerModel.ImageURL != banner.Image.ImageURL)
                    {
                        await imageService.DeactivateSingleImageByIdAsync(banner.ImageID);
                        var newImage = await imageService.AddNewImageAndReturnAsync(banerModel.ImageURL, banerModel.ImageURL);

                        banner.ImageID = newImage.Id;
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
        public async Task DeactivateBannerAsync(int bannerId)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var banner = await context.Banner.FirstAsync(x => x.IsActive && x.Id == bannerId);
                    banner.IsActive = false;

                    await imageService.DeactivateSingleImageByIdAsync(banner.ImageID);

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
