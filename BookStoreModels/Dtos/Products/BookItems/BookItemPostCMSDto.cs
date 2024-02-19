using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Products.BookItems
{
    public class BookItemPostCMSDto : BaseDto
    {
        [Required]
        public float Tax { get; set; }
        [Required]
        public decimal NettoPrice { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public DateTime PublishingDate { get; set; }
        [Required]
        public int Pages { get; set; }
        public int? TranslatorID { get; set; }
        public int? LanguageID { get; set; }
        public int? EditionID { get; set; }
        public int? FileFormatID { get; set; }
        [Required]
        public int FormID { get; set; }
        public int? AvailabilityID { get; set; }
        [Required]
        public int BookID { get; set; }
        public int? StockAmount { get; set; }
    }
}
