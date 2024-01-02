using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderItemsListViewModel : BaseViewModel
    {
        public int Quantity { get; set; }
        public decimal BruttoPrice { get; set; }
    }
}
