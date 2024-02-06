using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.BookItems.Review
{
    public class BookReviewViewModel : BaseViewModel
    {
        public string CustomerName { get; set; }
        public int ScoreValue { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Content { get; set; }
    }
}
