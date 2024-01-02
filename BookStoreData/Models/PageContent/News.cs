using BookStoreData.Models.Helpers;
using BookStoreData.Models.Media;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.PageContent
{
    public class News : BaseEntity
    {
        #region Properties
        public string? AuthorName { get; set; }
        public string? Topic { get; set; }
        public string? Content { get; set; }
        #endregion
        #region Foreign Keys
        //Image
        [Display(Name = "Zdjęcie")]
        public int? ImageID { get; set; }

        [ForeignKey("ImageID")]
        [JsonIgnore]
        public virtual Images Image { get; set; }
        #endregion
    }
}
