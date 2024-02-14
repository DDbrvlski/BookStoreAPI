using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Supply
{
    public class SupplyDto : BaseDto
    {
        public string SupplierName { get; set; }
        public DateTime SupplyDate { get; set; }
        public decimal PriceBrutto { get; set; }
    }
}
