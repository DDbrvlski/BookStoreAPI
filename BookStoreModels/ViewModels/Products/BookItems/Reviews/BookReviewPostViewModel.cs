using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.BookItems.Review
{
    public class BookReviewPostViewModel : BaseViewModel
    {
        public string Content { get; set; }
        public int ScoreId { get; set; }
        public int BookItemId { get; set; }
    }
}
