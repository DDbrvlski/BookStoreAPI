using BookStoreDto.Dtos.Customers;
using BookStoreDto.Dtos.Customers.Address;
using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Orders
{
    public class OrderPostDto
    {
        [Required]
        public int DeliveryMethodID { get; set; }

        [Required]
        public int PaymentMethodID { get; set; }

        public int? DiscountCodeID { get; set; }

        public CustomerGuestDto? CustomerGuest { get; set; }

        [Required]
        public BaseAddressDto InvoiceAddress { get; set; }

        public BaseAddressDto? DeliveryAddress { get; set; }

        public List<OrderItemsListDto>? CartItems { get; set; }
    }
}
