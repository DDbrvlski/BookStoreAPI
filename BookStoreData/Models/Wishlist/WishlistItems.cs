using BookStoreData.Models.Customers;
using BookStoreData.Models.Helpers;
using BookStoreData.Models.Products.BookItems;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Wishlist
{
    public class WishlistItems : BaseEntity
    {
        [Display(Name = "Książka")]
        public int? BookItemID { get; set; }

        [ForeignKey("BookItemID")]
        [JsonIgnore]
        public virtual BookItem? BookItem { get; set; }

        [Display(Name = "Wishlista")]
        public int? WishlistID { get; set; }

        [ForeignKey("WishlistID")]
        [JsonIgnore]
        public virtual Wishlist? Wishlist { get; set; }
    }
}
