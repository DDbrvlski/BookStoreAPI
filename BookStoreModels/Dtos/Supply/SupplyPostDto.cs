using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Supply
{
    public class SupplyPostDto
    {
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public int DeliveryStatusId { get; set; }
        [Required]
        public int PaymentMethodId { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }

        public List<SupplyBookPostDto>? BookItems { get; set; }
    }
}
