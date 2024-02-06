using BookStoreViewModels.ViewModels.Customers;
using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderViewModel : BaseViewModel
    {
        public CustomerShortDetailsViewModel CustomerDetails { get; set; }
        public decimal TotalBruttoPrice { get; set; }
        public DateTime OrderDate { get; set; } 
        public List<OrderItemDetailsViewModel> OrderItems { get; set; }
    }
}
