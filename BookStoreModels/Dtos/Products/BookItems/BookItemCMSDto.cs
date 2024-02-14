using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Products.BookItems
{
    public class BookItemCMSDto : BaseDto
    {
        public decimal NettoPrice { get; set; }
        public string ISBN { get; set; }
        public string FormName { get; set; }
        public int BookID { get; set; }
        public string BookTitle { get; set; }
    }
}
