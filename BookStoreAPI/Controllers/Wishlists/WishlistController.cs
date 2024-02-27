using BookStoreAPI.Services.Wishlists;
using BookStoreData.Data;
using BookStoreData.Models.Accounts;
using BookStoreDto.Dtos.Wishlists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Wishlist
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController(IWishlistService wishlistService) : ControllerBase
    {
        [HttpGet]
        [Route("{publicIdentifier}")]
        public async Task<ActionResult<WishlistDto>> GetUserWishlist(Guid publicIdentifier)
        {
            var wishlist = await wishlistService.GetUserWishlistAsync(publicIdentifier);

            return Ok(wishlist);
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<Guid>> GetUserWishlist()
        {
            var wishlistGuid = await wishlistService.GetUserGuidWishlistAsync();

            return Ok(wishlistGuid);
        }

        [HttpPost]
        [Route("Item")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> EditUserWishlistItem(int bookItemId, bool isWishlisted)
        {
            await wishlistService.UpdateWishlistAsync(bookItemId, isWishlisted);

            return NoContent();
        }
    }
}
