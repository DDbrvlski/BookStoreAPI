namespace BookStoreDto.Dtos.Supply
{
    public class SupplyPutDto
    {
        public int SupplierId { get; set; }
        public int DeliveryStatusId { get; set; }
        public DateTime DeliveryDate { get; set; }

        public List<SupplyBookPostDto>? BookItems { get; set; }
    }
}
