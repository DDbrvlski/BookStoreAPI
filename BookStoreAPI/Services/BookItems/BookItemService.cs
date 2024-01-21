using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Discounts.Discounts;
using BookStoreAPI.Services.Reviews;
using BookStoreAPI.Services.Stock;
using BookStoreAPI.Services.Wishlists;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreViewModels.ViewModels.Media.Images;
using BookStoreViewModels.ViewModels.Orders;
using BookStoreViewModels.ViewModels.Products.BookItems;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStoreAPI.Services.BookItems
{
    public interface IBookItemService
    {
        Task<BookItemDetailsCMSViewModel> GetBookItemByIdForCMSAsync(int bookItemId);
        Task<BookItemDetailsViewModel> GetBookItemDetailsAsync(int bookItemId);
        Task<IEnumerable<BookItemViewModel>> GetBookItemsAsync(BookItemFiltersViewModel bookItemFilters);
        Task<IEnumerable<BookItemCarouselViewModel>> GetBookItemsByFormIdForCarouselAsync(int formId);
        Task<IEnumerable<BookItemCMSViewModel>> GetBookItemsForCMSAync();
        Task CreateBookItemAsync(BookItemPostCMSViewModel bookItemModel);
        Task UpdateBookItemAsync(int bookItemId, BookItemPostCMSViewModel bookItemModel);
        Task DeactivateBookItemAsync(int bookItemId);
        Task<List<BookItemDiscountViewModel>> GetBookItemsFromOrderAsync(List<OrderItemsListViewModel> cartItems);
    }

    public class BookItemService
        (BookStoreContext context, 
        IBookReviewService bookReviewService, 
        IWishlistService wishlistService,
        IBookDiscountService bookDiscountService,
        IStockAmountService stockAmountService)
        : IBookItemService
    {
        public async Task<BookItemDetailsCMSViewModel> GetBookItemByIdForCMSAsync(int bookItemId)
        {
            return await context.BookItem
                .Where(x => x.IsActive && x.Id == bookItemId)
                .Select(element => new BookItemDetailsCMSViewModel
                {
                    Id = element.Id,
                    TranslatorName = element.Translator.Name + " " + element.Translator.Surname,
                    LanguageName = element.Language.Name,
                    EditionName = element.Edition.Name,
                    FileFormatName = element.FileFormat.Name,
                    FormName = element.Form.Name,
                    AvailabilityName = element.Availability.Name,
                    BookName = element.Book.Title,
                    BruttoPrice = element.NettoPrice * (1 + (decimal)(element.Tax / 100.0f)),
                    NettoPrice = element.NettoPrice,
                    VAT = element.Tax,
                    ISBN = element.ISBN,
                    Pages = element.Pages,
                    PublishingDate = element.PublishingDate,
                    TranslatorID = element.TranslatorID,
                    LanguageID = element.TranslatorID,
                    EditionID = element.EditionID,
                    BookID = element.BookID,
                    FileFormatID = element.FileFormatID,
                    FormID = element.FormID,
                    AvailabilityID = element.AvailabilityID,
                })
                .FirstAsync();
        }
        public async Task<IEnumerable<BookItemCMSViewModel>> GetBookItemsForCMSAync()
        {
            return await context.BookItem
                .Include(x => x.Book)
                .Include(x => x.Form)
                .Where(x => x.IsActive == true)
                .Select(x => new BookItemCMSViewModel
                {
                    Id = x.Id,
                    FormName = x.Form.Name,
                    BookTitle = x.Book.Title,
                    ISBN = x.ISBN,
                    BookID = x.BookID,
                    NettoPrice = x.NettoPrice
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<BookItemCarouselViewModel>> GetBookItemsByFormIdForCarouselAsync(int formId)
        {
            return await context.BookItem
                .Include(x => x.Form)
                .Where(x => x.IsActive == true && x.FormID == formId)
                .Select(x => new BookItemCarouselViewModel
                {
                    Id = x.Id,
                    Title = x.Book.Title,
                    ImageURL = x.Book.BookImages
                        .OrderBy(x => x.Image.Position)
                        .First(y => y.BookID == x.BookID).Image.ImageURL,
                    FormId = x.FormID,
                    FormName = x.Form.Name
                }).Take(25)
                .ToListAsync();
        }
        public async Task<IEnumerable<BookItemViewModel>> GetBookItemsAsync(BookItemFiltersViewModel bookItemFilters)
        {
            var items = context.BookItem
                        .Where(x => x.IsActive == true)
                        .ApplyBookFilters(bookItemFilters);

            var bookItems = await items.Select(x => new BookItemViewModel
            {
                Id = x.Id,
                ImageURL = x.Book.BookImages
                    .OrderBy(x => x.Image.Position)
                    .First(y => y.BookID == x.BookID).Image.ImageURL,
                Title = x.Book.Title,
                FormId = x.FormID,
                FormName = x.Form.Name,
                FileFormatId = x.FileFormatID,
                FileFormatName = x.FileFormat.Name,
                EditionId = x.EditionID,
                EditionName = x.Edition.Name,
                Price = x.NettoPrice * (1 + ((decimal)x.Tax / 100)),
                Score = x.Score,
                AvailabilityID = x.AvailabilityID,
                AvailabilityName = x.Availability.Name,
                Authors = x.Book.BookAuthors.Where(x => x.IsActive).Select(y => new AuthorViewModel
                {
                    Id = (int)y.AuthorID,
                    Name = y.Author.Name,
                    Surname = y.Author.Surname,
                }).ToList(),

            }).ToListAsync();


            bookItems = await bookDiscountService.ApplyDiscount(bookItems);

            return bookItems;
        }
        public async Task<BookItemDetailsViewModel> GetBookItemDetailsAsync(int bookItemId)
        {
            bool isWishlisted = await wishlistService.IsBookItemWishlistedByCustomer(bookItemId);
            var scoreValues = await bookReviewService.GetBookItemReviewScoresAsync(bookItemId);

            var bookItem = await context.BookItem
                .Where(x => x.Id == bookItemId && x.IsActive)
                .Select(x => new BookItemDetailsViewModel()
                {
                    Id = x.Id,
                    BookTitle = x.Book.Title,
                    BookId = x.BookID,
                    FormName = x.Form.Name,
                    Score = x.Score,
                    Pages = x.Pages,
                    FormId = x.FormID,
                    AvailabilityId = x.AvailabilityID,
                    AvailabilityName = x.Availability.Name,
                    Price = x.NettoPrice * (1 + ((decimal)x.Tax / 100)),
                    FileFormatName = x.FileFormat.Name,
                    EditionName = x.Edition.Name,
                    PublisherName = x.Book.Publisher.Name,
                    Language = x.Language.Name,
                    OriginalLanguage = x.Book.OriginalLanguage.Name,
                    TranslatorName = x.Translator.Name + " " + x.Translator.Surname,
                    ISBN = x.ISBN,
                    IsWishlisted = isWishlisted,
                    Description = x.Book.Description,
                    ReleaseDate = x.PublishingDate,
                    Authors = x.Book.BookAuthors.Select(y => new AuthorViewModel
                    {
                        Id = (int)y.AuthorID,
                        Name = y.Author.Name,
                        Surname = y.Author.Surname,
                    }).ToList(),
                    Categories = x.Book.BookCategories.Select(y => new CategoryViewModel
                    {
                        Id = (int)y.CategoryID,
                        Name = y.Category.Name
                    }).ToList(),
                    Images = x.Book.BookImages.Select(y => new ImageViewModel
                    {
                        Id = (int)y.ImageID,
                        ImageURL = y.Image.ImageURL,
                        Title = y.Image.Title
                    }).ToList(),
                    ScoreValues = scoreValues
                }).FirstAsync();

            var discount = await bookDiscountService.GetDiscountForBookItemAsync(bookItemId);

            if (discount != null)
            {
                bookItem.DiscountedBruttoPrice = bookItem.Price * (1 - (decimal)(discount.PercentOfDiscount / 100));
            }

            return bookItem;
        }
        public async Task CreateBookItemAsync(BookItemPostCMSViewModel bookItemModel)
        {
            BookItem bookItem = new();
            bookItem.CopyProperties(bookItemModel);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            await stockAmountService.CreateStockAmountAsync(bookItem.Id, bookItemModel.StockAmount);
            await UpdateBookItemAvailabilityAsync(bookItem, await stockAmountService.GetStockAmountForBookItemByIdAsync(bookItem.Id));
        }
        public async Task UpdateBookItemAsync(int bookItemId, BookItemPostCMSViewModel bookItemModel)
        {
            var bookItem = await context.BookItem.FirstOrDefaultAsync(x => x.Id == bookItemId && x.IsActive);
            if (bookItem == null)
            {
                throw new NotFoundException("Nie znaleziono elementu bookItem.");
            }

            bookItem.CopyProperties(bookItemModel);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            await UpdateBookItemAvailabilityAsync(bookItem, await stockAmountService.GetStockAmountForBookItemByIdAsync(bookItem.Id));
        }
        public async Task DeactivateBookItemAsync(int bookItemId)
        {
            var bookItem = await context.BookItem.FirstOrDefaultAsync(x => x.IsActive && x.Id == bookItemId);
            if (bookItem == null)
            {
                throw new NotFoundException("Nie znaleziono elementu bookItem.");
            }

            bookItem.IsActive = false;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            await bookDiscountService.DeactivateAllBookDiscountsByBookItemAsync(bookItemId);
            await stockAmountService.DeactivateStockAmountAsync(bookItemId);
        }
        private async Task UpdateBookItemAvailabilityAsync(BookItem bookItem, int stockAmount)
        {
            if (stockAmount == 0)
            {
                bookItem.AvailabilityID = 2;
            }
            else if (stockAmount > 0)
            {
                bookItem.AvailabilityID = 1;
            }
            else
            {
                throw new BadRequestException("Wystąpił błąd podczas aktualizowania stanu magazynowego produktu.");
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task<List<BookItemDiscountViewModel>> GetBookItemsFromOrderAsync(List<OrderItemsListViewModel> cartItems)
        {
            List<int> cartItemIds = cartItems.Select(x => x.BookItemID).ToList();
            return await context.BookItem
                .Where(x => cartItemIds.Contains(x.Id))
                .Select(x => new BookItemDiscountViewModel()
                {
                    BookItemId = x.Id,
                    BookItemQuantity = 1,
                    //BookItemDiscountedBruttoPrice = x.NettoPrice * (1 + ((decimal)x.Tax / 100)),
                    BookItemBruttoPrice = x.NettoPrice * (1 + ((decimal)x.Tax / 100)),
                })
                .ToListAsync();
        }
    }
}
