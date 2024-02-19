using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Users;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreDto.Dtos.Library;
using BookStoreDto.Dtos.Products.Books.Dictionaries;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Library
{
    public interface ILibraryService
    {
        Task<IEnumerable<LibraryItemsDto>> GetAllEbooksAvailableForUserAsync(int libraryStatusId);
        Task<byte[]> GetEbookPdfFileAsync(int bookItemId);
    }

    public class LibraryService
        (BookStoreContext context, 
        IUserContextService userContextService, 
        IWebHostEnvironment hostEnvironment)
        : ILibraryService
    {
        public async Task<IEnumerable<LibraryItemsDto>> GetAllEbooksAvailableForUserAsync(int libraryStatusId)
        {
            var user = await userContextService.GetUserByTokenAsync();
            if (user == null)
            {
                throw new UnauthorizedException("Należy się zalogować");
            }

            List<LibraryItemsDto>? rentedEbooks = new();
            List<LibraryItemsDto>? boughtEbooks = new();
            IEnumerable<LibraryItemsDto> allLibraryItems = null;

            if (libraryStatusId == 0 || libraryStatusId == 1)
            {
                rentedEbooks = await context.Rental
                .Where(x => x.CustomerID == user.CustomerID)
                .Select(x => new LibraryItemsDto()
                {
                    Id = (int)x.BookItemID,
                    BookTitle = x.BookItem.Book.Title,
                    ExpiryDate = x.EndDate,
                    FileFormatName = x.BookItem.FileFormat.Name,
                    FormName = x.BookItem.Form.Name,
                    ImageURL = x.BookItem.Book.BookImages
                            .First(x => x.Image.Position == 1).Image.ImageURL,
                    Authors = x.BookItem.Book.BookAuthors
                            .Where(y => y.IsActive)
                            .Select(y => new AuthorDto()
                            {
                                Id = (int)y.AuthorID,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList()
                })
                .GroupBy(x => x.Id)
                .Select(group => group.First())
                .ToListAsync();

                allLibraryItems = rentedEbooks;
            }

            if (libraryStatusId == 0 || libraryStatusId == 2)
            {
                boughtEbooks = await context.OrderItems
                    .Where(x => x.IsActive && x.Order.CustomerID == user.CustomerID && x.BookItem.FormID == 2)
                    .Select(x => new LibraryItemsDto()
                    {
                        Id = (int)x.BookItemID,
                        BookTitle = x.BookItem.Book.Title,
                        FileFormatName = x.BookItem.FileFormat.Name,
                        FormName = x.BookItem.Form.Name,
                        ImageURL = x.BookItem.Book.BookImages
                            .First(y => y.Image.Position == 1).Image.ImageURL,
                        Authors = x.BookItem.Book.BookAuthors
                            .Where(y => y.IsActive)
                            .Select(y => new AuthorDto()
                            {
                                Id = (int)y.AuthorID,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList()
                    })
                    .GroupBy(x => x.Id)
                    .Select(group => group.First())
                    .ToListAsync();

                allLibraryItems = boughtEbooks;                
            }

            if (libraryStatusId == 0)
            {
                allLibraryItems = rentedEbooks.Concat(boughtEbooks);
            }

            return allLibraryItems;
        }
        public async Task<byte[]> GetEbookPdfFileAsync(int bookItemId)
        {
            var ebook = await context.BookItem.Where(x => x.IsActive && x.Id == bookItemId).FirstOrDefaultAsync();

            if (ebook == null)
            {
                throw new BadRequestException("Nie znaleziono podanej książki.");
            }
            else if(ebook.FormID == 1)
            {
                throw new BadRequestException("Podane id przedmiotu nie jest ebookiem.");
            }

            var filePath = GetFilePathForEbook(ebook);

            if (filePath == null || !System.IO.File.Exists(filePath))
            {
                throw new NotFoundException("Nie znaleziono podanego pliku");
            }

            return System.IO.File.ReadAllBytes(filePath);
        }
        private string GetFilePathForEbook(BookItem book)
        {
            string rootPath = hostEnvironment.ContentRootPath;
            return Path.Combine(rootPath, "Files", "Ebooks", "PDFs", "Ebook.pdf");
        }
    }
}
