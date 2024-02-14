namespace BookStoreDto.Dtos.Statistics
{
    public class StatisticsBookItemsDetailsDto
    {
        public string BookTitle { get; set; }
        public string FormTitle { get; set; }
        public int SoldUnits { get; set; }
        public decimal SoldPrice { get; set; }
        public float PercentOfTotalSoldUnits { get; set; }
        public float PercentOfTotalSoldPrice { get; set; }
    }
}
