using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Orders
{
    public class OrderItemsListDto
    {
        public int BookItemID { get; set; }
        public int? BookFormID { get; set; }
        public int Quantity { get; set; }
        public decimal SingleItemBruttoPrice { get; set; } = 0;
        public decimal OriginalItemBruttoPrice { get; set; } = 0;
        public bool IsDiscounted { get; set; } = false;
    }
}
