using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Orders
{
    public class OrderCMSDto : BaseDto
    {
        public DateTime OrderDate { get; set; }
        public string OrderStatusName { get; set; }
        public int CustomerID { get; set; }
    }
}
