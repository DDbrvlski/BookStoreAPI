using BookStoreViewModels.ViewModels.Customers;
using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Orders.Dictionaries;
using BookStoreViewModels.ViewModels.Payments;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderDetailsViewModel : BaseViewModel
    {
        public decimal TotalBruttoPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DeliveryMethodViewModel? DeliveryMethod { get; set; }
        public OrderStatusViewModel? OrderStatus { get; set; }
        public PaymentDetailsViewModel? Payment { get; set; }
        //public ShippingDetailsViewModel? Shipping { get; set; }
        public CustomerShortDetailsViewModel? Customer { get; set; }

        public List<OrderItemDetailsViewModel> OrderItems { get; set; }
    }
}
