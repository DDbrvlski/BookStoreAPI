using BookStoreAPI.Helpers;
using BookStoreData.Data;
using BookStoreData.Models.Products.Books;
using BookStoreDto.Dtos.Products.Books;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Books.Dictionaries
{
    public interface ICategoryService
    {
        Task AddCategoriesForBookAsync(BookPostDto book, List<int>? categoryIds = null);
        Task UpdateCategoriesForBookAsync(BookPostDto book);
        Task DeactivateAllCategoriesForBookAsync(int? bookId);
        Task DeactivateChosenCategoriesForBookAsync(int? bookId, List<int> categoryIds);
    }
    public class CategoryService(BookStoreContext context) : ICategoryService
    {
        public async Task AddCategoriesForBookAsync(BookPostDto book, List<int>? categoryIds = null)
        {
            //Reużywalność funkcji dla dodawania nowych kategorii i aktualizowania
            if (categoryIds == null)
            {
                categoryIds = book.ListOfBookCategories.Select(x => x.Id).ToList();
            }

            if (categoryIds.Any())
            {
                var categoriesToAdd = categoryIds.Select(categoryId => new BookCategory
                {
                    CategoryID = categoryId,
                    BookID = book.Id
                }).ToList();

                await context.BookCategory.AddRangeAsync(categoriesToAdd);

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }

        public async Task UpdateCategoriesForBookAsync(BookPostDto book)
        {
            List<int> categoryIds = book.ListOfBookCategories.Select(x => x.Id).ToList();

            if (categoryIds.Any())
            {
                var existingCategoryIds = await context.BookCategory
                    .Where(x => x.BookID == book.Id && x.IsActive == true)
                    .Select(x => x.CategoryID)
                    .ToListAsync();

                var categoriesToDeactivate = existingCategoryIds.Except(categoryIds).ToList();
                var categoriesToAdd = categoryIds.Except(existingCategoryIds).ToList();

                if (categoriesToDeactivate.Any())
                {
                    await DeactivateChosenCategoriesForBookAsync(book.Id, categoriesToDeactivate);
                }

                if (categoriesToAdd.Any())
                {
                    await AddCategoriesForBookAsync(book, categoriesToAdd);
                }
            }
        }

        public async Task DeactivateAllCategoriesForBookAsync(int? bookId)
        {
            var categories = await context.BookCategory
                .Where(x => x.BookID == bookId && x.IsActive)
                .ToListAsync();

            if (categories.Any())
            {
                foreach (var category in categories)
                {
                    category.IsActive = false;
                }

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }

        public async Task DeactivateChosenCategoriesForBookAsync(int? bookId, List<int> categoryIds)
        {
            var categoriesToDeactivate = await context.BookCategory
                .Where(x => x.BookID == bookId && categoryIds.Contains(x.CategoryID) && x.IsActive == true)
                .ToListAsync();

            if (categoriesToDeactivate.Any())
            {
                foreach (var category in categoriesToDeactivate)
                {
                    category.IsActive = false;
                }

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
    }
}
