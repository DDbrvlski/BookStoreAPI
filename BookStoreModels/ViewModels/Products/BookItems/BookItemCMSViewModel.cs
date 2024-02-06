using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.BookItems
{
    public class BookItemCMSViewModel : BaseViewModel
    {
        public decimal NettoPrice { get; set; }
        public string ISBN { get; set; }
        public string FormName { get; set; }
        public int BookID { get; set; }
        public string BookTitle { get; set; }
    }
}
