using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Products.StockAmount
{
    public class StockAmountDto : BaseDto
    {
        public int Amount { get; set; }
        public int BookItemID { get; set; }
        public string BookTitle { get; set; }
    }
}
