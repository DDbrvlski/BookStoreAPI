using BookStoreDto.Dtos.Customers;
using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Orders.Dictionaries;
using BookStoreDto.Dtos.Payments;

namespace BookStoreDto.Dtos.Orders
{
    public class OrderDetailsDto : BaseDto
    {
        public decimal TotalBruttoPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DeliveryMethodDto? DeliveryMethod { get; set; }
        public OrderStatusDto? OrderStatus { get; set; }
        public PaymentDetailsDto? Payment { get; set; }
        public CustomerShortDetailsDto? Customer { get; set; }

        public List<OrderItemDetailsDto> OrderItems { get; set; }
    }
}
