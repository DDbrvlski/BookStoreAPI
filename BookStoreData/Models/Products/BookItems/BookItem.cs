using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookStoreData.Models.Products.Books;
using BookStoreData.Models.Products.BookItems.BookItemDictionaries;
using BookStoreData.Models.Products.Books.BookDictionaries;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Products.BookItems
{
    public class BookItem : BaseEntity
    {
        #region Properties
        public float VAT { get; set; }
        public double Score { get; set; } = 0;
        public decimal NettoPrice { get; set; }
        public int SoldUnits { get; set; } = 0;

        [MaxLength(13)]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Liczba stron jest wymagana.")]
        [Range(1, int.MaxValue, ErrorMessage = "Liczba stron musi wynosić więcej niż zero.")]
        public int Pages { get; set; }

        [Required(ErrorMessage = "Data publikacji jest wymagana.")]
        public DateTime PublishingDate { get; set; }
        #endregion
        #region Foreign Keys
        //Translator
        [Display(Name = "Tłumacz")]
        public int? TranslatorID { get; set; }

        [ForeignKey("TranslatorID")]
        [JsonIgnore]
        public virtual Translator Translator { get; set; }

        //Language
        [Required(ErrorMessage = "Język jest wymagany.")]
        [Display(Name = "Język")]
        public int? LanguageID { get; set; }

        [ForeignKey("LanguageID")]
        [JsonIgnore]
        public virtual Language Language { get; set; }

        //Book
        [Required(ErrorMessage = "Książka jest wymagana.")]
        [Display(Name = "Książka")]
        public int? BookID { get; set; }

        [ForeignKey("BookID")]
        [JsonIgnore]
        public virtual Book Book { get; set; }

        //Edition
        [Display(Name = "Edycja")]
        public int? EditionID { get; set; }

        [ForeignKey("EditionID")]
        [JsonIgnore]
        public virtual Edition Edition { get; set; }

        //FileFormat
        [Display(Name = "Format pliku")]
        public int? FileFormatID { get; set; }

        [ForeignKey("FileFormatID")]
        [JsonIgnore]
        public virtual FileFormat FileFormat { get; set; }

        //Form
        [Required(ErrorMessage = "Forma jest wymagana.")]
        [Display(Name = "Forma")]
        public int? FormID { get; set; }

        [ForeignKey("FormID")]
        [JsonIgnore]
        public virtual Form Form { get; set; }

        //Availability
        [Required(ErrorMessage = "Dostępność jest wymagana.")]
        [Display(Name = "Dostępność")]
        public int? AvailabilityID { get; set; }

        [ForeignKey("AvailabilityID")]
        [JsonIgnore]
        public virtual Availability Availability { get; set; }
        #endregion

        [JsonIgnore]
        public List<BookDiscount>? BookDiscounts { get; set; }
        [JsonIgnore]
        public List<BookItemReview>? BookItemReviews { get; set; }
    }
}
