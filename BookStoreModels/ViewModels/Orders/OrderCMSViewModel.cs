using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderCMSViewModel : BaseViewModel
    {
        public DateTime OrderDate { get; set; }
        public string? OrderStatusName { get; set; }
        public int? CustomerID { get; set; }
    }
}
