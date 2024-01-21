namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderDiscountCheckViewModel
    {
        public int? DiscountID { get; set; } = null;
        public string DiscountCode { get; set; }
        public List<OrderItemsListViewModel>? CartItems { get; set; }
    }
}
