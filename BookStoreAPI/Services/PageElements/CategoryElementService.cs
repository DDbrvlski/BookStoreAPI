using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Media;
using BookStoreData.Data;
using BookStoreData.Models.PageContent;
using BookStoreDto.Dtos.PageContent.Banners;
using BookStoreDto.Dtos.PageContent.CategoryElements;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.PageElements
{
    public interface ICategoryElementService
    {
        Task CreateCategoryElementAsync(CategoryElementDto categoryElementModel);
        Task DeactivateCategoryElementAsync(int categoryElementId);
        Task EditCategoryElementAsync(int categoryElementId, CategoryElementDto categoryElementModel);
        Task<IEnumerable<CategoryElementDto>> GetAllCategoryElementsAsync();
        Task<CategoryElementDto> GetCategoryElementByIdAsync(int categoryElementId);
    }
    public class CategoryElementService(BookStoreContext context, IImageService imageService) : ICategoryElementService
    {
        public async Task CreateCategoryElementAsync(CategoryElementDto categoryElementModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var newImage = await imageService.AddNewImageAndReturnAsync(categoryElementModel.ImageTitle, categoryElementModel.ImageURL);

                    CategoryElement newCategoryElement = new();
                    newCategoryElement.CopyProperties(categoryElementModel);
                    newCategoryElement.ImageID = newImage.Id;

                    await context.CategoryElement.AddAsync(newCategoryElement);
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
            
        }

        public async Task DeactivateCategoryElementAsync(int categoryElementId)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var categoryElement = await context.CategoryElement.FirstAsync(x => x.IsActive && x.Id == categoryElementId);
                    categoryElement.IsActive = false;

                    await imageService.DeactivateSingleImageByIdAsync(categoryElement.ImageID);

                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
            
        }

        public async Task EditCategoryElementAsync(int categoryElementId, CategoryElementDto categoryElementModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var categoryElement = await context.CategoryElement.Include(x => x.Image).FirstAsync(x => x.IsActive && x.Id == categoryElementId);
                    categoryElement.CopyProperties(categoryElementModel);

                    if (categoryElementModel.ImageURL != categoryElement.Image.ImageURL)
                    {
                        await imageService.DeactivateSingleImageByIdAsync(categoryElement.ImageID);
                        var newImage = await imageService.AddNewImageAndReturnAsync(categoryElementModel.ImageURL, categoryElementModel.ImageURL);

                        categoryElement.ImageID = newImage.Id;
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

        public async Task<IEnumerable<CategoryElementDto>> GetAllCategoryElementsAsync()
        {
            return await context.CategoryElement
                .Where(x => x.IsActive == true)
                .Select(x => new CategoryElementDto
                {
                    Id = x.Id,
                    Path = x.Path,
                    Content = x.Content,
                    Logo = x.Logo,
                    Position = x.Position,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL,
                    CategoryID = x.CategoryID,
                    CategoryName = x.Category.Name
                }).ToListAsync();
        }

        public async Task<CategoryElementDto> GetCategoryElementByIdAsync(int categoryElementId)
        {
            return await context.CategoryElement
                .Include(x => x.Image)
                .Include(x => x.Category)
                .Where(x => x.IsActive == true && x.Id == categoryElementId)
                .Select(x => new CategoryElementDto
                {
                    Id = x.Id,
                    Path = x.Path,
                    Content = x.Content,
                    Logo = x.Logo,
                    Position = x.Position,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL,
                    CategoryID = x.CategoryID,
                    CategoryName = x.Category.Name
                }).FirstAsync();
        }
    }
}
