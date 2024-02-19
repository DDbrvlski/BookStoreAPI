using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Rentals
{
    public class RentalPostDto : BaseDto
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int BookItemID { get; set; }
        [Required]
        public int PaymentMethodID { get; set; }
        [Required]
        public int RentalTypeID { get; set; }
    }
}
