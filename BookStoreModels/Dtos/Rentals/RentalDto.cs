using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Rentals
{
    public class RentalDto : BaseDto
    {
        public int BookItemId { get; set; }
        public string BookTitle { get; set; }
        public string? FileFormatName { get; set; }
        public string? ImageURL { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
