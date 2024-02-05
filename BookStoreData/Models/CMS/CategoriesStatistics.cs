using BookStoreData.Models.Helpers;
using BookStoreData.Models.Products.Books.BookDictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreData.Models.CMS
{
    public class CategoriesStatistics : BaseEntity
    {
        public int NumberOfAppearances { get; set; }
        public int StatisticsID { get; set; }
        [ForeignKey("StatisticsID")]
        public virtual Statistics Statistics { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
    }
}
