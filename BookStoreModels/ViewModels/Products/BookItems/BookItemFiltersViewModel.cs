using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.BookItems
{
    public class BookItemFiltersViewModel : BaseViewModel
    {
        public string? SearchPhrase { get; set; }
        public List<int?>? AuthorIds { get; set; }
        public List<int?>? CategoryIds { get; set; }
        public List<int?>? FormIds { get; set; }
        public List<int?>? PublisherIds { get; set; }
        public List<int?>? LanguageIds { get; set; }
        public List<int?>? ScoreValues { get; set; }
        public List<int?>? AvailabilitiesIds { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public bool? IsOnSale { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public int? NumberOfElements { get; set; }
        public int? BookId { get; set; }
    }
}
