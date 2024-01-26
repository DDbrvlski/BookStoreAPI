using BookStoreData.Models.Helpers;
using BookStoreData.Models.Products.BookItems;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreData.Models.Supplies
{
    public class SupplyGoods : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal BruttoPrice { get; set; }
        public int BookItemID { get; set; }

        [ForeignKey("BookItemID")]
        public virtual BookItem BookItem { get; set; }

        public int SupplyID { get; set; }

        [ForeignKey("SupplyID")]
        public virtual Supply Supply { get; set; }
    }
}
