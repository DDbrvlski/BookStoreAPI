using BookStoreDto.Dtos.Customers;
using BookStoreDto.Dtos.Customers.Address;
using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Orders
{
    public class OrderPostDto
    {
        public int DeliveryMethodID { get; set; }
        public int PaymentMethodID { get; set; }
        public int? DiscountCodeID { get; set; }
        public CustomerGuestDto? CustomerGuest { get; set; }
        public BaseAddressDto InvoiceAddress { get; set; }
        public BaseAddressDto? DeliveryAddress { get; set; }

        public List<OrderItemsListDto>? CartItems { get; set; }
    }
}
