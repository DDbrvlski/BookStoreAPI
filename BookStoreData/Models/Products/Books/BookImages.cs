using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookStoreData.Models.Media;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.Products.Books
{
    public class BookImages : BaseEntity
    {
        //Image
        [Required(ErrorMessage = "Zdjęcie jest wymagane.")]
        [Display(Name = "Zdjęcie")]
        public int? ImageID { get; set; }

        [ForeignKey("ImageID")]
        [JsonIgnore]
        public virtual Images Image { get; set; }

        //Book
        [Required(ErrorMessage = "Książka jest wymagana.")]
        [Display(Name = "Książka")]
        public int? BookID { get; set; }

        [ForeignKey("BookID")]
        [JsonIgnore]
        public virtual Book Book { get; set; }
    }
}
