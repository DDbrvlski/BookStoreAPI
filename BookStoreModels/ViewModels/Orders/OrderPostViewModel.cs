using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Payments;
using BookStoreViewModels.ViewModels.Shippings.Dictionaries;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderPostViewModel : BaseViewModel
    {
        public DateTime OrderDate { get; set; }
        public int? CustomerID { get; set; }
        public int? OrderStatusID { get; set; }
        public int? DeliveryMethodID { get; set; }
        public PaymentPostViewModel? Payment { get; set; }
        public ShippingPostViewModel? Shipping { get; set; }

        public List<OrderItemsListViewModel>? ListOfOrderItems { get; set; }
    }
}
