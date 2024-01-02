using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookStoreData.Models.Accounts;
using BookStoreData.Models.Products.Books.BookDictionaries;
using System.Text.Json.Serialization;
using BookStoreData.Models.Customers;

namespace BookStoreData.Models.Products.BookItems
{
    public class BookItemReview : BaseEntity
    {
        public string? Content { get; set; }

        //User
        [Required(ErrorMessage = "Użytkownik jest wymagany.")]
        [Display(Name = "Użytkownik")]
        public int? CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        [JsonIgnore]
        public virtual Customer Customer { get; set; }

        //Book
        [Required(ErrorMessage = "Książka jest wymagana.")]
        [Display(Name = "Książka")]
        public int? BookItemID { get; set; }

        [ForeignKey("BookItemID")]
        [JsonIgnore]
        public virtual BookItem BookItem { get; set; }

        //Score
        [Required(ErrorMessage = "Ocena jest wymagana.")]
        [Display(Name = "Ocena")]
        public int? ScoreID { get; set; }

        [ForeignKey("ScoreID")]
        [JsonIgnore]
        public virtual Score Score { get; set; }
    }
}
