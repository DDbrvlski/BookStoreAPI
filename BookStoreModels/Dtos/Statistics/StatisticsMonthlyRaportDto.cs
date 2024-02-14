namespace BookStoreDto.Dtos.Statistics
{
    public class StatisticsMonthlyRaportDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int SoldQuantity { get; set; }
        public decimal GrossRevenue { get; set; }
        public decimal GrossExpenses { get; set; }
        public decimal TotalDiscounts { get; set; }
        public decimal TotalIncome { get; set; }
        public List<StatisticsBookItemsDetailsDto> BookItems { get; set; }
        public List<StatisticsCategoriesDetailsDto> Categories { get; set; }
    }
}
