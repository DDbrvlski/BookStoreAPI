using BookStoreViewModels.ViewModels.Customers;
using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Orders.Dictionaries;
using BookStoreViewModels.ViewModels.Payments;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderDetailsCMSViewModel : BaseViewModel
    {
        public DateTime OrderDate { get; set; }
        public DeliveryMethodViewModel? DeliveryMethod { get; set; }
        public OrderStatusViewModel? OrderStatus { get; set; }
        public PaymentDetailsViewModel? Payment { get; set; }
        public CustomerShortDetailsViewModel? Customer { get; set; }

        public List<OrderItemDetailsViewModel>? ListOfOrderItems { get; set; }
    }
}
