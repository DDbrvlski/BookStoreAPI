using BookStoreDto.Dtos.Helpers;
using BookStoreDto.Dtos.Products.BookItems;

namespace BookStoreDto.Dtos.Orders
{
    public class OrderItemDetailsCMSDto : BaseDto
    {
        public int Quantity { get; set; }
        public decimal BruttoPrice { get; set; }
        public int BookItemID { get; set; }
        public int OrderID { get; set; }
        public BookItemInOrderDto? ItemForOrder { get; set; }
    }
}
