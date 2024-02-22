using BookStoreDto.Dtos.Customers;
using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Orders
{
    public class OrderDto : BaseDto
    {
        public CustomerShortDetailsDto CustomerDetails { get; set; }
        public decimal TotalBruttoPrice { get; set; }
        public DateTime OrderDate { get; set; } 
        public int OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }
        public List<OrderItemDetailsDto> OrderItems { get; set; }
    }
}
