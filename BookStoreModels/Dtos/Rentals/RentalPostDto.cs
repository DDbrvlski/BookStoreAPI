using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Rentals
{
    public class RentalPostDto : BaseDto
    {
        public DateTime StartDate { get; set; }
        public int BookItemID { get; set; }
        public int PaymentMethodID { get; set; }
        public int RentalTypeID { get; set; }
    }
}
