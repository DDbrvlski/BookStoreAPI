using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Rentals
{
    public class RentalViewModel : BaseViewModel
    {
        public int? BookItemId { get; set; }
        public string? BookTitle { get; set; }
        public string? FileFormatName { get; set; }
        public string? ImageURL { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
