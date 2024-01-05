using BookStoreViewModels.ViewModels.Customers;
using BookStoreViewModels.ViewModels.Customers.Address;
using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderPostViewModel
    {
        public int? DeliveryMethodID { get; set; }
        public int? PaymentMethodID { get; set; }
        public int? DiscountCodeID { get; set; }
        public CustomerGuestViewModel? CustomerGuest { get; set; }
        public BaseAddressViewModel InvoiceAddress { get; set; }
        public BaseAddressViewModel? DeliveryAddress { get; set; }

        public List<OrderItemsListViewModel>? CartItems { get; set; }
    }
}
