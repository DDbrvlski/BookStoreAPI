using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Discounts.Discounts;
using BookStoreAPI.Services.Users;
using BookStoreData.Data;
using BookStoreData.Models.Customers;
using BookStoreData.Models.Wishlist;
using BookStoreDto.Dtos.Products.Books.Dictionaries;
using BookStoreDto.Dtos.Wishlists;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Wishlists
{
    public interface IWishlistService
    {
        Task<Wishlist> CreateWishlistAsync(Customer customer);
        Task<WishlistDto> GetUserWishlistAsync(Guid publicIdentifier);
        Task<Guid> GetUserGuidWishlistAsync();
        Task UpdateWishlistAsync(int bookItemId, bool isWishlisted);
        Task DeactivateWishlistAsync(int customerId);
        Task<bool> IsBookItemWishlistedByCustomer(int bookItemId);
        Task<Wishlist?> GetWishlistByDataAsync(Func<Wishlist, bool> wishlistFunction);
    }
    public class WishlistService(BookStoreContext context, IUserContextService userContextService, IBookDiscountService bookDiscountService) : IWishlistService
    {
        public async Task<WishlistDto> GetUserWishlistAsync(Guid publicIdentifier)
        {
            var isPublicWishlist = await context.Wishlist
                .Where(x => x.PublicIdentifier == publicIdentifier && x.IsActive)
                .Select(x => new { IsPublic = x.IsPublic, CustomerID = x.CustomerID })
                .FirstOrDefaultAsync();

            if (!isPublicWishlist.IsPublic)
            {
                var user = await userContextService.GetUserByTokenAsync();
                if (user == null || user.CustomerID != isPublicWishlist.CustomerID)
                {
                    throw new WishlistException("Brak dostępu do wishlisty.");
                }
            }

            var wishlistToSend = await context.Wishlist
                .Where(x => x.PublicIdentifier == publicIdentifier && x.IsActive)
                .Select(x => new WishlistDto()
                {
                    Id = x.Id,
                    IsPublic = x.IsPublic,
                    Items = x.WishlistItems
                        .Where(y => y.WishlistID == x.Id && y.IsActive)
                        .Select(y => new WishlistItemDto()
                        {
                            Id = (int)y.BookItemID,
                            BookTitle = y.BookItem.Book.Title,
                            EditionName = y.BookItem.Edition.Name,
                            FormName = y.BookItem.Form.Name,
                            FormId = y.BookItem.FormID,
                            FileFormatName = y.BookItem.FileFormat.Name,
                            ImageURL = y.BookItem.Book.BookImages.Where(z => z.Image.Position == 1).FirstOrDefault().Image.ImageURL,
                            BruttoPrice = y.BookItem.NettoPrice * (1 + ((decimal)y.BookItem.Tax / 100)),
                            authors = y.BookItem.Book.BookAuthors.Select(y => new AuthorDto
                            {
                                Id = (int)y.AuthorID,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList(),
                        }).ToList(),
                    FullPrice = x.WishlistItems
                        .Where(y => y.WishlistID == x.Id && y.IsActive)
                        .Sum(y => y.BookItem.NettoPrice * (1 + ((decimal)y.BookItem.Tax / 100)))
                }).FirstOrDefaultAsync();

            if (wishlistToSend == null)
            {
                throw new WishlistException("Wystąpił błąd podczas pobierania wishlisty.");
            }

            wishlistToSend.Items = await bookDiscountService.ApplyDiscount(wishlistToSend.Items);

            return wishlistToSend;
        }
        public async Task<Guid> GetUserGuidWishlistAsync()
        {
            var user = await userContextService.GetUserByTokenAsync();
            if (user == null)
            {
                throw new AccountException("Nie znaleziono użytkownika.");
            }

            var wishlistGuid = await context.Wishlist.Where(x => x.IsActive && x.CustomerID == user.CustomerID).Select(x => x.PublicIdentifier).FirstOrDefaultAsync();

            if (wishlistGuid == default)
            {
                throw new WishlistException("Wystąpił błąd podczas pobierania identyfikatora wishlisty.");
            }

            return wishlistGuid;
        }
        public async Task<Wishlist> CreateWishlistAsync(Customer customer)
        {
            Wishlist wishlist = new()
            {
                CustomerID = customer.Id,
                PublicIdentifier = Guid.NewGuid()
            };

            await context.Wishlist.AddAsync(wishlist);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            return wishlist;
        }
        public async Task UpdateWishlistAsync(int bookItemId, bool isWishlisted)
        {
            var user = await userContextService.GetUserByTokenAsync();
            if (user == null)
            {
                throw new AccountException("Nie znaleziono użytkownika.");
            }

            var userWishlistId = await context.Wishlist.Where(x => x.CustomerID == user.CustomerID && x.IsActive).Select(x => x.Id).FirstAsync();

            if (!isWishlisted)
            {
                WishlistItems wishlistItem = new()
                {
                    BookItemID = bookItemId,
                    WishlistID = userWishlistId,
                };
                context.WishlistItems.Add(wishlistItem);
            }
            else
            {
                var userWishlistItem = await context.WishlistItems.FirstAsync(x => x.WishlistID == userWishlistId && x.IsActive && x.BookItemID == bookItemId);
                if (userWishlistItem != null)
                {
                    userWishlistItem.IsActive = false;
                }
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task<bool> IsBookItemWishlistedByCustomer(int bookItemId)
        {
            var user = await userContextService.GetUserByTokenAsync();

            bool isWishlisted = false;

            if (user != null)
            {
                isWishlisted = await context.WishlistItems.AnyAsync(x => x.IsActive && x.Wishlist.CustomerID == user.CustomerID && x.BookItemID == bookItemId);
                //var wishlist = await context.Wishlist.FirstAsync(x => x.IsActive && x.CustomerID == user.CustomerID);
                //isWishlisted = await context.WishlistItems
                //    .AnyAsync(x => x.IsActive && x.WishlistID == wishlist.Id && x.BookItemID == bookItemId);
            }

            return isWishlisted;
        }
        public async Task DeactivateWishlistAsync(int customerId)
        {
            var wishlist = await GetWishlistByDataAsync(x => x.CustomerID == customerId);

            if (wishlist == null)
            {
                throw new BadRequestException("Wystąpił błąd.");
            }

            wishlist.IsActive = false;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task<Wishlist?> GetWishlistByDataAsync(Func<Wishlist, bool> wishlistFunction)
        {
            return await context.Wishlist.FirstOrDefaultAsync(x => wishlistFunction(x) && x.IsActive);
        }
    }
}
