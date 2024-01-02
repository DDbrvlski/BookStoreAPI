using BookStoreData.Models.Helpers;
using BookStoreData.Models.Products.BookItems;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Orders
{
    public class OrderItems : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal BruttoPrice { get; set; }

        //BookItem
        [Required(ErrorMessage = "Książka jest wymagana.")]
        [Display(Name = "Książka")]
        public int? BookItemID { get; set; }

        [ForeignKey("BookItemID")]
        [JsonIgnore]
        public virtual BookItem BookItem { get; set; }

        //Order
        [Required(ErrorMessage = "Zamówienie jest wymagane.")]
        [Display(Name = "Zamówienie")]
        public int? OrderID { get; set; }

        [ForeignKey("OrderID")]
        [JsonIgnore]
        public virtual Order Order { get; set; }
    }
}
