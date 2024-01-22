﻿using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Users;
using BookStoreData.Data;
using BookStoreViewModels.ViewModels.Library;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Library
{
    public interface ILibraryService
    {
        Task<IEnumerable<LibraryItemsViewModel>> GetAllEbooksAvailableForUserAsync();
    }

    public class LibraryService(BookStoreContext context, IUserContextService userContextService) : ILibraryService
    {
        public async Task<IEnumerable<LibraryItemsViewModel>> GetAllEbooksAvailableForUserAsync()
        {
            var user = await userContextService.GetUserByTokenAsync();
            if (user == null)
            {
                throw new UnauthorizedException("Należy się zalogować");
            }

            var ebooks = await context.Rental
                .Where(x => x.CustomerID == user.CustomerID)
                .Select(x => new LibraryItemsViewModel()
                {
                    Id = (int)x.BookItemID,
                    BookTitle = x.BookItem.Book.Title,
                    FileFormatName = x.BookItem.FileFormat.Name,
                    FormName = x.BookItem.Form.Name,
                    ImageURL = x.BookItem.Book.BookImages
                            .First(x => x.Image.Position == 1).Image.ImageURL,
                    Authors = x.BookItem.Book.BookAuthors
                            .Where(y => y.IsActive)
                            .Select(y => new AuthorViewModel()
                            {
                                Id = (int)y.AuthorID,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList()
                })
                .ToListAsync();

            var ebookss = await context.Order.Where(x => x.IsActive && x.CustomerID == user.CustomerID)
                .Select(x => x.OrderItems
                    .Where(y => y.BookItem.FormID == 2)
                    .Select(y => new LibraryItemsViewModel()
                    {
                        Id = (int)y.BookItemID,
                        BookTitle = y.BookItem.Book.Title,
                        FileFormatName = y.BookItem.FileFormat.Name,
                        FormName = y.BookItem.Form.Name,
                        ImageURL = y.BookItem.Book.BookImages
                            .First(z => z.Image.Position == 1).Image.ImageURL,
                        Authors = y.BookItem.Book.BookAuthors
                            .Where(z => z.IsActive)
                            .Select(z => new AuthorViewModel()
                            {
                                Id = (int)z.AuthorID,
                                Name = z.Author.Name,
                                Surname = z.Author.Surname,
                            }).ToList()
                    }))
                .ToListAsync();

            var allLibraryItems = ebooks.Concat(ebookss.SelectMany(x => x));
            return allLibraryItems;
        }
    }
}
