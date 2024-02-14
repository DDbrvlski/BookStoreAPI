using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Wishlists
{
    public class WishlistDto : BaseDto
    {
        public decimal FullPrice { get; set; }
        public bool IsPublic { get; set; }
        public List<WishlistItemDto>? Items { get; set; }
    }
}
