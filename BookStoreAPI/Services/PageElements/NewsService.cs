using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Customers;
using BookStoreAPI.Services.Media;
using BookStoreData.Data;
using BookStoreData.Models.PageContent;
using BookStoreDto.Dtos.PageContent.News;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.PageElements
{
    public interface INewsService
    {
        Task CreateNewsAsync(NewsPostCMSDto newsModel);
        Task DeactivateNewsAsync(int newsId);
        Task EditNewsAsync(int newsId, NewsPostCMSDto newsModel);
        Task<IEnumerable<NewsDto>> GetAllNewsAsync();
        Task<NewsDetailsDto> GetNewsByIdAsync(int newsId);
        Task<IEnumerable<NewsDto>> GetNumberOfNewsAsync(int numberOfElements);
    }

    public class NewsService(BookStoreContext context, IImageService imageService, ICustomerService customerService) : INewsService
    {
        public async Task<IEnumerable<NewsDto>> GetAllNewsAsync()
        {
            return await context.News
                .Include(x => x.Image)
                .Where(x => x.IsActive == true)
                .OrderByDescending(x => x.Id)
                .Select(x => new NewsDto
                {
                    Id = x.Id,
                    Topic = x.Topic,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).ToListAsync();
        }
        public async Task<IEnumerable<NewsDto>> GetNumberOfNewsAsync(int numberOfElements)
        {
            return await context.News
                .Include(x => x.Image)
                .Where(x => x.IsActive == true)
                .OrderByDescending(x => x.Id)
                .Select(x => new NewsDto
                {
                    Id = x.Id,
                    Topic = x.Topic,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).Take(numberOfElements).ToListAsync();
        }
        public async Task<NewsDetailsDto> GetNewsByIdAsync(int newsId)
        {
            return await context.News
                .Include(x => x.Image)
                .Where(x => x.IsActive && x.Id == newsId)
                .Select(x => new NewsDetailsDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    Topic = x.Topic,
                    AuthorName = x.AuthorName,
                    CreationDate = x.CreationDate,
                    ImageTitle = x.Image.Title,
                    ImageURL = x.Image.ImageURL
                }).FirstAsync();
        }
        public async Task CreateNewsAsync(NewsPostCMSDto newsModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var employee = await customerService.GetCustomerByTokenAsync();

                    var newImage = await imageService.AddNewImageAndReturnAsync(newsModel.ImageTitle, newsModel.ImageURL);

                    News news = new();
                    news.CopyProperties(newsModel);
                    news.ImageID = newImage.Id;
                    news.AuthorName = $"{employee.Name} {employee.Surname}";

                    await context.News.AddAsync(news);
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }            
        }
        public async Task EditNewsAsync(int newsId, NewsPostCMSDto newsModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var employee = await customerService.GetCustomerByTokenAsync();
                    var news = await context.News.Include(x => x.Image).FirstAsync(x => x.IsActive && x.Id == newsId);
                    news.CopyProperties(newsModel);
                    news.AuthorName = $"{employee.Name} {employee.Surname}";
                    news.ModifiedDate = DateTime.Now;

                    if (newsModel.ImageURL != news.Image.ImageURL)
                    {
                        await imageService.DeactivateSingleImageByIdAsync(news.ImageID);
                        var newImage = await imageService.AddNewImageAndReturnAsync(newsModel.ImageURL, newsModel.ImageURL);

                        news.ImageID = newImage.Id;
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
        public async Task DeactivateNewsAsync(int newsId)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var news = await context.News.FirstAsync(x => x.Id == newsId);
                    news.IsActive = false;
                    news.ModifiedDate = DateTime.Now;
                    await imageService.DeactivateSingleImageByIdAsync(news.ImageID);

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
