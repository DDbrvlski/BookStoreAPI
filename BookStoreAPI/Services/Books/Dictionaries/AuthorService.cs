using BookStoreAPI.Helpers;
using BookStoreData.Data;
using BookStoreData.Models.Products.Books;
using BookStoreDto.Dtos.Products.Books;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Books.Dictionaries
{
    public interface IAuthorService
    {
        Task AddAuthorsForBookAsync(BookPostDto book, List<int>? authorIds = null);
        Task UpdateAuthorsForBookAsync(BookPostDto book);
        Task DeactivateAllAuthorsForBookAsync(int? bookId);
        Task DeactivateChosenAuthorsForBookAsync(int? bookId, List<int> authorIds);
    }
    public class AuthorService(BookStoreContext context) : IAuthorService
    {
        public async Task AddAuthorsForBookAsync(BookPostDto book, List<int>? authorIds = null)
        {
            //Możliwość ponownego użycia funkcji dla dodawania nowych autorów i aktualizowania
            if(authorIds == null)
            {
                authorIds = book.ListOfBookAuthors.Select(x => x.Id).ToList();
            }

            if (authorIds.Any())
            {
                var authorsToAdd = authorIds.Select(authorId => new BookAuthor
                {
                    AuthorID = authorId,
                    BookID = book.Id
                }).ToList();

                await context.BookAuthor.AddRangeAsync(authorsToAdd);

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }

        public async Task UpdateAuthorsForBookAsync(BookPostDto book)
        {
            List<int> authorIds = book.ListOfBookAuthors.Select(x => x.Id).ToList();

            if (authorIds.Any())
            {
                var existingAuthorIds = await context.BookAuthor
                .Where(x => x.BookID == book.Id && x.IsActive == true)
                .Select(x => x.AuthorID)
                .ToListAsync();

                var authorsToDeactivate = existingAuthorIds.Except(authorIds).ToList();
                var authorsToAdd = authorIds.Except(existingAuthorIds).ToList();

                if (authorsToDeactivate.Any())
                {
                    await DeactivateChosenAuthorsForBookAsync(book.Id, authorsToDeactivate);
                }

                if (authorsToAdd.Any())
                {
                    await AddAuthorsForBookAsync(book, authorsToAdd);
                }
            }
        }

        public async Task DeactivateAllAuthorsForBookAsync(int? bookId)
        {
            var authors = await context.BookAuthor
                .Where(x => x.BookID == bookId && x.IsActive)
                .ToListAsync();

            if (authors.Any())
            {
                foreach (var author in authors)
                {
                    author.IsActive = false;
                    author.ModifiedDate = DateTime.UtcNow;
                }

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }

        public async Task DeactivateChosenAuthorsForBookAsync(int? bookId, List<int> authorIds)
        {
            var authorsToDeactivate = await context.BookAuthor
                .Where(x => x.BookID == bookId && authorIds.Contains(x.AuthorID) && x.IsActive == true)
                .ToListAsync();

            if (authorsToDeactivate.Any())
            {
                foreach (var author in authorsToDeactivate)
                {
                    author.IsActive = false;
                    author.ModifiedDate = DateTime.UtcNow;
                }

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
    }
}
