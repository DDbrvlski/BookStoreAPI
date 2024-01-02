using BookStoreData.Models.Helpers;
using BookStoreData.Models.Products.Books.BookDictionaries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Products.Books
{
    public class BookAuthor : BaseEntity
    {
        //Author
        [Required(ErrorMessage = "Autor jest wymagany.")]
        [Display(Name = "Autor")]
        public int? AuthorID { get; set; }

        [ForeignKey("AuthorID")]
        [JsonIgnore]
        public virtual Author Author { get; set; }

        //Book
        [Required(ErrorMessage = "Książka jest wymagana.")]
        [Display(Name = "Książka")]
        public int? BookID { get; set; }

        [ForeignKey("BookID")]
        [JsonIgnore]
        public virtual Book Book { get; set; }
    }
}
