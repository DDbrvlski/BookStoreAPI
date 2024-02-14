using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Supply
{
    public class SupplyPostDto
    {
        public int SupplierId { get; set; }
        public int DeliveryStatusId { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime DeliveryDate { get; set; }

        public List<SupplyBookPostDto>? BookItems { get; set; }
    }
}
