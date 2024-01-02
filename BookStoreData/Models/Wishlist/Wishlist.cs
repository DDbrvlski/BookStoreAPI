using BookStoreData.Models.Customers;
using BookStoreData.Models.Helpers;
using BookStoreData.Models.Products.Books.BookDictionaries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Wishlist
{
    public class Wishlist : BaseEntity
    {
        public Guid PublicIdentifier { get; set; }
        public bool IsPublic { get; set; } = true;
        [Display(Name = "Klient")]
        public int? CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        [JsonIgnore]
        public virtual Customer? Customer { get; set; }


        [JsonIgnore]
        public List<WishlistItems>? WishlistItems { get; set; }
    }
}
