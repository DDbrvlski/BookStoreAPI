namespace BookStoreDto.Dtos.Orders
{
    public class OrderDiscountCheckDto
    {
        public int? DiscountID { get; set; } = null;
        public string DiscountCode { get; set; }
        public List<OrderItemsListDto>? CartItems { get; set; }
    }
}
