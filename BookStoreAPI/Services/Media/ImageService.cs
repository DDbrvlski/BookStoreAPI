using BookStoreAPI.Helpers;
using BookStoreData.Data;
using BookStoreData.Models.Media;
using BookStoreData.Models.Products.Books;
using BookStoreViewModels.ViewModels.Media.Images;
using BookStoreViewModels.ViewModels.Products.Books;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Media
{
    public interface IImageService
    {
        Task<Images> AddNewImageAndReturnAsync(string title, string imageURL);
        Task DeactivateSingleImageByIdAsync(int? imageId);
        Task AddNewImagesForBookAsync(BookPostViewModel book, List<ImageViewModel?>? imagesToAdd = null);
        Task UpdateImagesForBookAsync(BookPostViewModel book);
        Task DeactivateAllImagesForBookAsync(int? bookId);
        Task DeactivateChosenImagesForBookAsync(int? bookId, List<int?> imageIds);
    }
    public class ImageService(BookStoreContext context) : IImageService
    {
        public async Task<Images> AddNewImageAndReturnAsync(string title, string imageURL)
        {
            Images NewImage = new Images()
            {
                Title = title,
                ImageURL = imageURL
            };
            await context.Images.AddAsync(NewImage);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            return NewImage;
        }

        public async Task AddNewImagesForBookAsync(BookPostViewModel book, List<ImageViewModel?>? imagesToAdd = null)
        {
            //Reużywalność kodu
            if (imagesToAdd == null)
            {
                imagesToAdd = book.ListOfBookImages.ToList();
            }

            if (imagesToAdd != null)
            {
                var newImages = imagesToAdd
                    .Where(image => image != null)
                    .Select(image => new Images
                    {
                        Title = image.Title,
                        ImageURL = image.ImageURL,
                        Position = image.Position
                    }).ToList();

                context.Images.AddRange(newImages);

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                var bookImages = newImages
                    .Select(image => new BookImages
                    {
                        ImageID = image.Id,
                        BookID = book.Id
                    }).ToList();

                context.BookImages.AddRange(bookImages);

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }

        public async Task DeactivateAllImagesForBookAsync(int? bookId)
        {
            var bookImages = await context.BookImages
                .Where(x => x.BookID == bookId && x.IsActive == true)
                .ToListAsync();

            foreach (var bookImage in bookImages)
            {
                bookImage.IsActive = false;
                bookImage.Image.IsActive = false; 
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }


        public async Task DeactivateChosenImagesForBookAsync(int? bookId, List<int?> imageIds)
        {
            var bookImages = await context.BookImages
                .Where(x => x.BookID == bookId && imageIds.Contains(x.ImageID) && x.IsActive == true)
                .ToListAsync();

            foreach (var bookImage in bookImages)
            {
                bookImage.IsActive = false;
                bookImage.Image.IsActive = false;
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task DeactivateSingleImageByIdAsync(int? imageId)
        {
            Images image = await context.Images.FirstOrDefaultAsync(x => x.Id == imageId);

            if (image != null)
            {
                image.IsActive = false;

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }

        public async Task UpdateImagesForBookAsync(BookPostViewModel book)
        {
            List<ImageViewModel> images = book.ListOfBookImages.ToList();

            var existingImageIds = await context.BookImages
                .Where(x => x.BookID == book.Id && x.IsActive == true)
                .Select(x => x.ImageID)
                .ToListAsync();

            var imageIds = images.Select(x => (int?)x.Id).ToList();

            var imagesToDeactivate = existingImageIds.Except(imageIds).ToList();
            var imagesToAdd = images.Where(x => x != null && !existingImageIds.Contains(x.Id)).ToList();

            if (imagesToDeactivate.Any())
            {
                await DeactivateChosenImagesForBookAsync(book.Id, imagesToDeactivate);
            }

            if (imagesToAdd.Any())
            {
                await AddNewImagesForBookAsync(book, imagesToAdd);
            }
        }
    }
}
