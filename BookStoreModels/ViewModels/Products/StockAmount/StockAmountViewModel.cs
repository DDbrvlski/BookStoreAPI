using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Products.StockAmount
{
    public class StockAmountViewModel : BaseViewModel
    {
        public int Amount { get; set; }
        public int BookItemID { get; set; }
        public string BookTitle { get; set; }
    }
}
