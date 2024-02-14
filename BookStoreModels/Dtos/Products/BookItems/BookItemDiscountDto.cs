namespace BookStoreDto.Dtos.Products.BookItems
{
    public class BookItemDiscountDto
    {
        public int BookItemId { get; set; }
        public decimal BookItemBruttoPrice { get; set; }
        //public decimal BookItemDiscountedBruttoPrice { get; set; }
        public int BookItemQuantity { get; set; }
    }
}
