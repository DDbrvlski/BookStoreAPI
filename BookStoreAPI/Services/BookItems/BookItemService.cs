using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Availability;
using BookStoreAPI.Services.Discounts.Discounts;
using BookStoreAPI.Services.Reviews;
using BookStoreAPI.Services.Stock;
using BookStoreAPI.Services.Wishlists;
using BookStoreData.Data;
using BookStoreData.Models.Orders;
using BookStoreData.Models.Products.BookItems;
using BookStoreDto.Dtos.Media.Images;
using BookStoreDto.Dtos.Orders;
using BookStoreDto.Dtos.Products.BookItems;
using BookStoreDto.Dtos.Products.Books.Dictionaries;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStoreAPI.Services.BookItems
{
    public interface IBookItemService
    {
        Task<BookItemDetailsCMSDto> GetBookItemByIdForCMSAsync(int bookItemId);
        Task<BookItemDetailsDto> GetBookItemDetailsAsync(int bookItemId);
        Task<IEnumerable<BookItemDto>> GetBookItemsAsync(BookItemFiltersDto bookItemFilters);
        Task<IEnumerable<BookItemCarouselDto>> GetBookItemsByFormIdForCarouselAsync(int formId);
        Task<IEnumerable<BookItemCMSDto>> GetBookItemsForCMSAync();
        Task CreateBookItemAsync(BookItemPostCMSDto bookItemModel);
        Task UpdateBookItemAsync(int bookItemId, BookItemPostCMSDto bookItemModel);
        Task DeactivateBookItemAsync(int bookItemId);
        Task ManageSoldUnitsAsync(List<OrderItems> orderItems);
        Task<List<BookItemDiscountDto>> GetBookItemsFromOrderAsync(List<OrderItemsListDto> cartItems);
    }

    public class BookItemService
        (BookStoreContext context, 
        IBookReviewService bookReviewService, 
        IWishlistService wishlistService,
        IBookDiscountService bookDiscountService,
        IStockAmountService stockAmountService)
        : IBookItemService
    {
        public async Task<BookItemDetailsCMSDto> GetBookItemByIdForCMSAsync(int bookItemId)
        {
            return await context.BookItem
                .Where(x => x.IsActive && x.Id == bookItemId)
                .Select(element => new BookItemDetailsCMSDto
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
                    Tax = element.Tax,
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
        public async Task<IEnumerable<BookItemCMSDto>> GetBookItemsForCMSAync()
        {
            return await context.BookItem
                .Include(x => x.Book)
                .Include(x => x.Form)
                .Where(x => x.IsActive == true)
                .Select(x => new BookItemCMSDto
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
        public async Task<IEnumerable<BookItemCarouselDto>> GetBookItemsByFormIdForCarouselAsync(int formId)
        {
            return await context.BookItem
                .Include(x => x.Form)
                .Where(x => x.IsActive == true && x.FormID == formId)
                .Select(x => new BookItemCarouselDto
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
        public async Task<IEnumerable<BookItemDto>> GetBookItemsAsync(BookItemFiltersDto bookItemFilters)
        {
            var items = context.BookItem
                        .Where(x => x.IsActive == true)
                        .ApplyBookFilters(bookItemFilters);

            var bookItems = await items.Select(x => new BookItemDto
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
                Authors = x.Book.BookAuthors.Where(x => x.IsActive).Select(y => new AuthorDto
                {
                    Id = y.AuthorID,
                    Name = y.Author.Name,
                    Surname = y.Author.Surname,
                }).ToList(),

            }).ToListAsync();


            bookItems = await bookDiscountService.ApplyDiscount(bookItems);

            return bookItems;
        }
        public async Task<BookItemDetailsDto> GetBookItemDetailsAsync(int bookItemId)
        {
            bool isWishlisted = await wishlistService.IsBookItemWishlistedByCustomer(bookItemId);
            var scoreValues = await bookReviewService.GetBookItemReviewScoresAsync(bookItemId);

            var bookItem = await context.BookItem
                .Where(x => x.Id == bookItemId && x.IsActive)
                .Select(x => new BookItemDetailsDto()
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
                    Authors = x.Book.BookAuthors.Select(y => new AuthorDto
                    {
                        Id = (int)y.AuthorID,
                        Name = y.Author.Name,
                        Surname = y.Author.Surname,
                    }).ToList(),
                    Categories = x.Book.BookCategories.Select(y => new CategoryDto
                    {
                        Id = (int)y.CategoryID,
                        Name = y.Category.Name
                    }).ToList(),
                    Images = x.Book.BookImages.Select(y => new ImageDto
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
        public async Task CreateBookItemAsync(BookItemPostCMSDto bookItemModel)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    BookItem bookItem = new();
                    bookItem.CopyProperties(bookItemModel);
                    bookItem.AvailabilityID = 2;
                    await context.BookItem.AddAsync(bookItem);

                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    if (bookItem.FormID == 1)
                    {
                        await stockAmountService.CreateStockAmountAsync(bookItem.Id, bookItemModel.StockAmount);
                    }
                    else if (bookItem.FormID == 2)
                    {
                        await stockAmountService.CreateStockAmountAsync(bookItem.Id, 0);
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }            
        }
        public async Task UpdateBookItemAsync(int bookItemId, BookItemPostCMSDto bookItemModel)
        {
            var bookItem = await context.BookItem.FirstOrDefaultAsync(x => x.Id == bookItemId && x.IsActive);
            if (bookItem == null)
            {
                throw new NotFoundException("Nie znaleziono elementu bookItem.");
            }

            bookItem.CopyProperties(bookItemModel);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateBookItemAsync(int bookItemId)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var bookItem = await context.BookItem.FirstOrDefaultAsync(x => x.IsActive && x.Id == bookItemId);
                    if (bookItem == null)
                    {
                        throw new NotFoundException("Nie znaleziono elementu bookItem.");
                    }

                    await bookDiscountService.DeactivateAllBookDiscountsByBookItemAsync(bookItemId);
                    await stockAmountService.DeactivateStockAmountAsync(bookItemId);

                    bookItem.IsActive = false;
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }            
        }
        public async Task ManageSoldUnitsAsync(List<OrderItems> orderItems)
        {
            var bookItemIds = orderItems.Select(x => x.BookItemID).ToList();
            var bookItems = await context.BookItem.Where(x => x.IsActive && bookItemIds.Contains(x.Id)).ToListAsync();

            foreach (var bookItem in bookItems)
            {
                var orderItem = orderItems.Where(x => x.BookItemID == bookItem.Id).First();

                bookItem.SoldUnits += orderItem.Quantity;
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task<List<BookItemDiscountDto>> GetBookItemsFromOrderAsync(List<OrderItemsListDto> cartItems)
        {
            List<int> cartItemIds = cartItems.Select(x => x.BookItemID).ToList();
            return await context.BookItem
                .Where(x => cartItemIds.Contains(x.Id))
                .Select(x => new BookItemDiscountDto()
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
