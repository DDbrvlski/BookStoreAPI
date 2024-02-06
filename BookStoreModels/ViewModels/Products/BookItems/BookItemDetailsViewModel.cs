using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Media.Images;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;

namespace BookStoreViewModels.ViewModels.Products.BookItems
{
    public class BookItemDetailsViewModel : BaseViewModel
    {
        public string BookTitle { get; set; }
        public int BookId { get; set; }
        public int FormId { get; set; }
        public int AvailabilityId { get; set; }
        public int Pages { get; set; }
        public string FormName { get; set; }
        public double Score { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedBruttoPrice { get; set; } = 0;
        public string? FileFormatName { get; set; }
        public string? EditionName { get; set; }
        public string PublisherName { get; set; }
        public string AvailabilityName { get; set; }
        public string? Language { get; set; }
        public string OriginalLanguage { get; set; }
        public string? TranslatorName { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public bool IsWishlisted { get; set; } = false;
        public DateTime ReleaseDate { get; set; }
        public List<AuthorViewModel>? Authors { get; set; }
        public List<CategoryViewModel>? Categories { get; set; }
        public List<ImageViewModel>? Images { get; set; }
        public Dictionary<int, int>? ScoreValues { get; set; }
    }
}
