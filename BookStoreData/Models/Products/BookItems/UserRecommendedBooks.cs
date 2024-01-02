using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookStoreData.Models.Accounts;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Products.BookItems
{
    public class UserRecommendedBooks : BaseEntity
    {
        public int Position { get; set; }

        //BookItem
        [Required(ErrorMessage = "Książka jest wymagana.")]
        [Display(Name = "Książka")]
        public int? BookItemID { get; set; }

        [ForeignKey("BookItemID")]
        [JsonIgnore]
        public virtual BookItem BookItem { get; set; }

        //User
        [Required(ErrorMessage = "Użytkownik jest wymagany.")]
        [Display(Name = "Użytkownik")]
        public string? UserID { get; set; }

        [ForeignKey("UserID")]
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
