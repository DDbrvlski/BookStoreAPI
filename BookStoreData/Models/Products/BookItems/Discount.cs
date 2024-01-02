using BookStoreData.Models.Helpers;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Products.BookItems
{
    public class Discount : BaseEntity
    {
        public string Title { get; set; }
        public decimal PercentOfDiscount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime StartingDate { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public List<BookItem>? BookItems { get; set; }
        [JsonIgnore]
        public List<BookDiscount>? BookDiscounts { get; set; }
    }
}
