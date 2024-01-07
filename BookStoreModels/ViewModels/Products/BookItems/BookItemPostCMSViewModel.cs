using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.BookItems
{
    public class BookItemPostCMSViewModel : BaseViewModel
    {
        public float VAT { get; set; }
        public decimal NettoPrice { get; set; }
        public string? ISBN { get; set; }
        public DateTime PublishingDate { get; set; }
        public int Pages { get; set; }
        public int? TranslatorID { get; set; }
        public int? LanguageID { get; set; }
        public int? EditionID { get; set; }
        public int? FileFormatID { get; set; }
        public int? FormID { get; set; }
        public int? AvailabilityID { get; set; }
        public int? BookID { get; set; }
        public int? StockAmount { get; set; }
    }
}
