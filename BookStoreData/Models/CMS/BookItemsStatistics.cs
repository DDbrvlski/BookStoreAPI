using BookStoreData.Models.Customers;
using BookStoreData.Models.Helpers;
using BookStoreData.Models.Products.BookItems;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreData.Models.CMS
{
    public class BookItemsStatistics : BaseEntity
    {
        public int SoldQuantity { get; set; }
        public decimal SoldPrice { get; set; }
        public int StatisticsID { get; set; }
        [ForeignKey("StatisticsID")]
        public virtual Statistics Statistics { get; set; }
        public int BookItemID { get; set; }
        [ForeignKey("BookItemID")]
        public virtual BookItem BookItem { get; set; }
    }
}
