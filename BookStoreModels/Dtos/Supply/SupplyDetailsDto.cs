using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Payments;

namespace BookStoreDto.Dtos.Supply
{
    public class SupplyDetailsDto : BaseDto
    {
        public DateTime? DeliveryDate { get; set; }
        public int DeliveryStatusId { get; set; }
        public string DeliveryStatusName { get; set; }
        public SupplierDto SupplierData { get; set; }
        public PaymentDetailsDto PaymentData { get; set; }
        public List<SupplyBooksDto> SupplyBooksData { get; set; }
    }
}
