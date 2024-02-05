namespace BookStoreViewModels.ViewModels.Statistics
{
    public class StatisticsMonthlyRaportViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int SoldQuantity { get; set; }
        public decimal GrossRevenue { get; set; }
        public decimal GrossExpenses { get; set; }
        public decimal TotalDiscounts { get; set; }
        public decimal TotalIncome { get; set; }
        public List<StatisticsBookItemsDetailsViewModel> BookItems { get; set; }
        public List<StatisticsCategoriesDetailsViewModel> Categories { get; set; }
    }
}
