using BookStoreData.Models.Helpers;
using BookStoreData.Models.Media;
using BookStoreData.Models.Products.Books.BookDictionaries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.PageContent
{
    public class CategoryElement : DictionaryTable
    {
        public string? Path { get; set; }
        public string? Logo { get; set; }
        public string? Content { get; set; }
        public int? Position { get; set; }
        #region Foreign Keys
        //Image
        [Display(Name = "Zdjęcie")]
        public int? ImageID { get; set; }

        [ForeignKey("ImageID")]
        [JsonIgnore]
        public virtual Images Image { get; set; }

        [Display(Name = "Kategoria")]
        public int? CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        [JsonIgnore]
        public virtual Category Category { get; set; }
        #endregion
    }
}
