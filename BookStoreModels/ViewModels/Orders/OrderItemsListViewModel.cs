using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderItemsListViewModel
    {
        public int BookItemID { get; set; }
        public int? Quantity { get; set; }
        public decimal? SingleItemBruttoPrice { get; set; } = 0;
        public decimal? OriginalItemBruttoPrice { get; set; } = 0;
        public bool IsDiscounted { get; set; } = false;
    }
}
