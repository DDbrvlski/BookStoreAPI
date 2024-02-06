using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.BookItems
{
    public class BookItemDetailsCMSViewModel : BaseViewModel
    {
        public decimal BruttoPrice { get; set; }
        public decimal NettoPrice { get; set; }
        public float Tax { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
        public DateTime PublishingDate { get; set; }
        public string? TranslatorName { get; set; }
        public string? LanguageName { get; set; }
        public string BookName { get; set; }
        public string? EditionName { get; set; }
        public string? FileFormatName { get; set; }
        public string FormName { get; set; }
        public string AvailabilityName { get; set; }
        public int? TranslatorID { get; set; }
        public int? LanguageID { get; set; }
        public int BookID { get; set; }
        public int? EditionID { get; set; }
        public int? FileFormatID { get; set; }
        public int FormID { get; set; }
        public int AvailabilityID { get; set; }
    }
}
