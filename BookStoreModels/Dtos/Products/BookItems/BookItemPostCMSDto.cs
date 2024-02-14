using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Products.BookItems
{
    public class BookItemPostCMSDto : BaseDto
    {
        public float Tax { get; set; }
        public decimal NettoPrice { get; set; }
        public string ISBN { get; set; }
        public DateTime PublishingDate { get; set; }
        public int Pages { get; set; }
        public int? TranslatorID { get; set; }
        public int? LanguageID { get; set; }
        public int? EditionID { get; set; }
        public int? FileFormatID { get; set; }
        public int FormID { get; set; }
        public int? AvailabilityID { get; set; }
        public int BookID { get; set; }
        public int? StockAmount { get; set; }
    }
}
