using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;

namespace BookStoreViewModels.ViewModels.Products.Reservations
{
    public class ReservationBookItemViewModel : BaseViewModel
    {
        public string? ImageURL { get; set; }
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public double Score { get; set; }
        public string? FormName { get; set; }
        public string? FileFormatName { get; set; }
        public string? EditionName { get; set; }
        public List<AuthorViewModel> Authors { get; set; } = new List<AuthorViewModel>();
    }
}
