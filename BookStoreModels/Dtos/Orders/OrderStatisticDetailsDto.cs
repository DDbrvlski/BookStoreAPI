using BookStoreDto.Dtos.Statistics;

namespace BookStoreDto.Dtos.Orders
{
    public class OrderStatisticDetailsDto
    {
        public int SoldQuantity { get; set; }
        public decimal GrossRevenue { get; set; }
        public decimal TotalDiscounts { get; set; }
        public List<StatisticsCategoriesDto> TopCategoryIds { get; set; }
        public List<StatisticsBookItemsDto> TopBookItemIds { get; set; }
    }
}
