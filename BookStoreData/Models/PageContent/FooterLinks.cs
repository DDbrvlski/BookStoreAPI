using BookStoreData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.PageContent
{
    public class FooterLinks : DictionaryTable
    {
        public string? Path { get; set; }
        public string? URL { get; set; }
        public int? Position { get; set; }

        //FooterColumn
        [Display(Name = "Kolumna")]
        public int? FooterColumnID { get; set; }

        [ForeignKey("FooterColumnID")]
        [JsonIgnore]
        public virtual FooterColumns? FooterColumn { get; set; }
    }
}
