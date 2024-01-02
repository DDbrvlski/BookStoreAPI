using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Orders.Dictionaries;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderDetailsViewModel : BaseViewModel
    {
        public decimal FullBruttoPrice { get; set; }
        public List<OrderItemDetailsViewModel> OrderItems { get; set; }
        public DeliveryMethodViewModel? DeliveryMethod { get; set; }
    }
}
