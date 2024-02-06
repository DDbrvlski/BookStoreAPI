using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;

namespace BookStoreViewModels.ViewModels.Supply
{
    public class SupplyBooksViewModel
    {
        public int BookItemId { get; set; }
        public string BookTitle { get; set; }
        public string FormName { get; set; }
        public string? EditionName { get; set; }
        public string? FileFormatName { get; set; }
        public decimal BruttoPrice { get; set; }
        public int Quantity { get; set; }
        public List<AuthorViewModel> Authors { get; set; }
    }
}
