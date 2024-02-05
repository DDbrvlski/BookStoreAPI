using BookStoreData.Models.Helpers;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.CMS
{
    public class Statistics : BaseEntity
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int SoldQuantity { get; set; }
        public decimal GrossRevenue { get; set; }
        public decimal GrossExpenses { get; set; }
        public decimal TotalDiscounts { get; set; }

        [JsonIgnore]
        public List<BookItemsStatistics>? BookItemsStatistics { get; set; }

        [JsonIgnore]
        public List<CategoriesStatistics>? CategoriesStatistics { get; set; }
    }
}
