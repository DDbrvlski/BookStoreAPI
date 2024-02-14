using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Books.Dictionaries;
using BookStoreAPI.Services.Media;
using BookStoreData.Data;
using BookStoreData.Models.Products.Books;
using BookStoreDto.Dtos.Media.Images;
using BookStoreDto.Dtos.Products.Books;
using BookStoreDto.Dtos.Products.Books.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreAPI.Services.Books
{
    public interface IBookService
    {
        Task<BookDetailsCMSDto> GetBookDetailsForCMSByIdAsync(int id);
        Task<IEnumerable<BooksCMSDto>> GetAllBooksForCMSAsync();
        Task CreateBookAsync(BookPostDto bookPost);
        Task DeactivateBookAsync(int bookId);
        Task UpdateBookAsync(int bookId, BookPostDto bookPost);
    }
    public class BookService
        (BookStoreContext context, 
        ILogger<BookService> logger, 
        IAuthorService authorService,
        ICategoryService categoryService,
        IImageService imageService)
        : IBookService
    {
        public async Task<BookDetailsCMSDto> GetBookDetailsForCMSByIdAsync(int id)
        {
            return await context.Book
                .Where(x => x.Id == id && x.IsActive)
                .Select(element => new BookDetailsCMSDto
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
                            .Select(y => new CategoryDto
                            {
                                Id = y.Category.Id,
                                Name = y.Category.Name,
                            }).ToList(),
                    Authors = element.BookAuthors
                            .Where(z => z.IsActive == true)
                            .Select(y => new AuthorDto
                            {
                                Id = y.Author.Id,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList(),
                    Images = element.BookImages
                            .Where(y => y.IsActive == true)
                            .Select(y => new ImageDto
                            {
                                Id = y.Image.Id,
                                Title = y.Image.Title,
                                ImageURL = y.Image.ImageURL,
                            }).ToList(),
                })
                .FirstAsync();
        }
        public async Task<IEnumerable<BooksCMSDto>> GetAllBooksForCMSAsync()
        {
            return await context.Book
                .Where(x => x.IsActive == true)
                .Select(x => new BooksCMSDto
                {
                    Id = x.Id,
                    PublisherName = x.Publisher.Name,
                    Title = x.Title,
                    Authors = x.BookAuthors
                            .Where(y => y.IsActive == true)
                            .Select(y => new AuthorDto
                            {
                                Id = y.Author.Id,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList()
                })
                .ToListAsync();
        }
        public async Task CreateBookAsync(BookPostDto bookPost)
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
            using (var transaction = context.Database.BeginTransaction())
            {
                try
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
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }            
        }
        public async Task UpdateBookAsync(int bookId, BookPostDto bookPost)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var bookToUpdate = await context.Book.FirstOrDefaultAsync(x => x.Id == bookId);
                    bookToUpdate.CopyProperties(bookPost);

                    if(bookToUpdate != null)
                    {
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
