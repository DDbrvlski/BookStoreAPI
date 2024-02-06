using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;

namespace BookStoreViewModels.ViewModels.Products.BookItems
{
    public class BookItemViewModel : BaseViewModel
    {
        public string? ImageURL { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public double Score { get; set; }
        public int FormId { get; set; }
        public string FormName { get; set; }
        public int? FileFormatId { get; set; }
        public string? FileFormatName { get; set; }
        public int? EditionId { get; set; }
        public string? EditionName { get; set; }
        public string AvailabilityName { get; set; }
        public int AvailabilityID { get; set; }
        public decimal DiscountedBruttoPrice { get; set; } = 0;
        public List<AuthorViewModel> Authors { get; set; } = new List<AuthorViewModel>();
    }
}
