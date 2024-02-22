using BookStoreAPI.Helpers;
using BookStoreData.Data;
using BookStoreData.Models.Media;
using BookStoreData.Models.Products.Books;
using BookStoreDto.Dtos.Media.Images;
using BookStoreDto.Dtos.Products.Books;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Media
{
    public interface IImageService
    {
        Task<Images> AddNewImageAndReturnAsync(string title, string imageURL);
        Task DeactivateSingleImageByIdAsync(int? imageId);
        Task AddNewImagesForBookAsync(BookPostDto book, List<ImageDto?>? imagesToAdd = null);
        Task UpdateImagesForBookAsync(BookPostDto book);
        Task DeactivateAllImagesForBookAsync(int? bookId);
        Task DeactivateChosenImagesForBookAsync(int? bookId, List<int> imageIds);
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

        public async Task AddNewImagesForBookAsync(BookPostDto book, List<ImageDto?>? imagesToAdd = null)
        {
            //Reużywalność kodu dla update
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

                await context.Images.AddRangeAsync(newImages);

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
                .Include(x => x.Image)
                .Where(x => x.BookID == bookId && x.IsActive == true)
                .ToListAsync();

            foreach (var bookImage in bookImages)
            {
                bookImage.IsActive = false;
                bookImage.ModifiedDate = DateTime.UtcNow;
                bookImage.Image.IsActive = false; 
                bookImage.Image.ModifiedDate = DateTime.UtcNow; 
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }


        public async Task DeactivateChosenImagesForBookAsync(int? bookId, List<int> imageIds)
        {
            var bookImages = await context.BookImages
                .Include(x => x.Image)
                .Where(x => x.BookID == bookId && imageIds.Contains(x.ImageID) && x.IsActive == true)
                .ToListAsync();

            foreach (var bookImage in bookImages)
            {
                bookImage.IsActive = false;
                bookImage.ModifiedDate = DateTime.UtcNow;
                bookImage.Image.IsActive = false;
                bookImage.Image.ModifiedDate = DateTime.UtcNow;
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task DeactivateSingleImageByIdAsync(int? imageId)
        {
            Images image = await context.Images.FirstOrDefaultAsync(x => x.Id == imageId);

            if (image != null)
            {
                image.IsActive = false;
                image.ModifiedDate = DateTime.UtcNow;
                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }

        public async Task UpdateImagesForBookAsync(BookPostDto book)
        {
            List<ImageDto> images = book.ListOfBookImages.ToList();

            var existingImageIds = await context.BookImages
                .Where(x => x.BookID == book.Id && x.IsActive == true)
                .Select(x => x.ImageID)
                .ToListAsync();

            var imageIds = images.Select(x => x.Id).ToList();

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
