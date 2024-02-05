using BookStoreViewModels.ViewModels.Statistics;

namespace BookStoreViewModels.ViewModels.Orders
{
    public class OrderStatisticDetailsViewModel
    {
        public int SoldQuantity { get; set; }
        public decimal GrossRevenue { get; set; }
        public decimal TotalDiscounts { get; set; }
        public List<StatisticsCategoriesViewModel> TopCategoryIds { get; set; }
        public List<StatisticsBookItemsViewModel> TopBookItemIds { get; set; }
    }
}
