using BookStoreData.Models.Helpers;
using BookStoreData.Models.Products.BookItems;
using BookStoreData.Models.Products.Books.BookDictionaries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Products.Books
{
    public class Book : BaseEntity
    {
        #region Properties
        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }
        #endregion
        #region Foreign Keys

        //OriginalLanguage
        [Required(ErrorMessage = "Oryginalny język jest wymagany.")]
        [Display(Name = "Oryginalny język")]
        public int? OriginalLanguageID { get; set; }

        [ForeignKey("OriginalLanguageID")]
        [JsonIgnore]
        public virtual Language? OriginalLanguage { get; set; }

        //Publisher
        [Required(ErrorMessage = "Wydawca jest wymagany.")]
        [Display(Name = "Wydawca")]
        public int? PublisherID { get; set; }

        [ForeignKey("PublisherID")]
        [JsonIgnore]
        public virtual Publisher? Publisher { get; set; }
        #endregion
        #region Navigation properties
        [JsonIgnore]
        public List<BookItem>? BookItems { get; set; }
        [JsonIgnore]
        public List<BookAuthor>? BookAuthors { get; set; }
        [JsonIgnore]
        public List<BookCategory>? BookCategories { get; set; }
        [JsonIgnore]
        public List<BookDiscount>? BookDiscounts { get; set; }
        [JsonIgnore]
        public List<BookImages>? BookImages { get; set; }
        #endregion
    }
}
