namespace BookStoreDto.Dtos.Supply
{
    public class SupplyBookPostDto
    {
        public int BookItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal BruttoPrice { get; set; }
    }
}
