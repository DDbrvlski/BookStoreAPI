using BookStoreAPI.Helpers;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Books.Dictionaries;
using BookStoreAPI.Services.Media;
using BookStoreData.Data;
using BookStoreData.Models.Products.Books;
using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Media.Images;
using BookStoreViewModels.ViewModels.Products.Books;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreAPI.Services.Books
{
    public interface IBookService
    {
        Task<BookDetailsCMSViewModel> GetBookDetailsForCMSByIdAsync(int id);
        Task<IEnumerable<BooksCMSViewModel>> GetAllBooksForCMSAsync();
        Task CreateBookAsync(BookPostViewModel bookPost);
        Task DeactivateBookAsync(int bookId);
        Task UpdateBookAsync(int bookId, BookPostViewModel bookPost);
    }
    public class BookService
        (BookStoreContext context, 
        ILogger<BookService> logger, 
        IAuthorService authorService,
        ICategoryService categoryService,
        IImageService imageService)
        : IBookService
    {
        public async Task<BookDetailsCMSViewModel> GetBookDetailsForCMSByIdAsync(int id)
        {
            return await context.Book
                .Where(x => x.Id == id && x.IsActive)
                .Select(element => new BookDetailsCMSViewModel
                {
                    Id = element.Id,
                    OriginalLanguageName = element.OriginalLanguage.Name,
                    PublisherName = element.Publisher.Name,
                    Description = element.Description,
                    OriginalLanguageID = element.OriginalLanguageID,
                    PublisherID = element.PublisherID,
                    Title = element.Title,
                    Categories = element.BookCategories
                            .Where(z => z.IsActive == true)
                            .Select(y => new CategoryViewModel
                            {
                                Id = y.Category.Id,
                                Name = y.Category.Name,
                            }).ToList(),
                    Authors = element.BookAuthors
                            .Where(z => z.IsActive == true)
                            .Select(y => new AuthorViewModel
                            {
                                Id = y.Author.Id,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList(),
                    Images = element.BookImages
                            .Where(y => y.IsActive == true)
                            .Select(y => new ImageViewModel
                            {
                                Id = y.Image.Id,
                                Title = y.Image.Title,
                                ImageURL = y.Image.ImageURL,
                            }).ToList(),
                })
                .FirstAsync();
        }
        public async Task<IEnumerable<BooksCMSViewModel>> GetAllBooksForCMSAsync()
        {
            return await context.Book
                .Where(x => x.IsActive == true)
                .Select(x => new BooksCMSViewModel
                {
                    Id = x.Id,
                    PublisherName = x.Publisher.Name,
                    Title = x.Title,
                    Authors = x.BookAuthors
                            .Where(y => y.IsActive == true)
                            .Select(y => new AuthorViewModel
                            {
                                Id = y.Author.Id,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList()
                })
                .ToListAsync();
        }
        public async Task CreateBookAsync(BookPostViewModel bookPost)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Book newBook = new();
                    newBook.CopyProperties(bookPost);

                    await context.Book.AddAsync(newBook);
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    bookPost.Id = newBook.Id;
                    await authorService.AddAuthorsForBookAsync(bookPost);
                    await categoryService.AddCategoriesForBookAsync(bookPost);
                    await imageService.AddNewImagesForBookAsync(bookPost);

                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    transaction.Commit();
                }
                catch (InvalidOperationException ex)
                {
                    transaction.Rollback();
                    logger.LogError($"{ex.Message}");
                    throw new InvalidOperationException("Wystąpił błąd");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.LogError($"{ex.Message}");
                    throw new BadRequestException("Wystąpił błąd podczas aktualizacji bazy danych");
                }
            }
        }
        public async Task DeactivateBookAsync(int id)
        {
            var book = await context.Book.FirstOrDefaultAsync(x => x.Id == id);

            if (book != null)
            {
                await authorService.DeactivateAllAuthorsForBookAsync(id);
                await categoryService.DeactivateAllCategoriesForBookAsync(id);
                await imageService.DeactivateAllImagesForBookAsync(id);
                book.IsActive = false;

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
        public async Task UpdateBookAsync(int bookId, BookPostViewModel bookPost)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var bookToUpdate = await context.Book.FirstOrDefaultAsync(x => x.Id == bookId);

                    if(bookToUpdate != null)
                    {
                        bookToUpdate.IsActive = false;
                        bookPost.Id = bookId;
                        if (!bookPost.ListOfBookAuthors.IsNullOrEmpty())
                        {
                            await authorService.UpdateAuthorsForBookAsync(bookPost);
                        }
                        if (!bookPost.ListOfBookCategories.IsNullOrEmpty())
                        {
                            await categoryService.UpdateCategoriesForBookAsync(bookPost);
                        }
                        if (!bookPost.ListOfBookImages.IsNullOrEmpty())
                        {
                            await imageService.UpdateImagesForBookAsync(bookPost);
                        }

                        await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                        transaction.Commit();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    transaction.Rollback();
                    logger.LogError($"{ex.Message}");
                    throw new InvalidOperationException("Wystąpił błąd");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.LogError($"{ex.Message}");
                    throw new BadRequestException("Wystąpił błąd podczas aktualizacji bazy danych");
                }
            }
        }
    }
}
