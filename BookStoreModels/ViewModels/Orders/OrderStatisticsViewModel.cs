namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderStatisticsViewModel
    {
        public int SoldQuantity { get; set; }
        public decimal GrossRevenue { get; set; }
        public decimal TotalDiscounts { get; set; }
        public List<int?> CategoryIDs { get; set; }
        public int BookItemID { get; set; }
    }
}
