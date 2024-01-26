using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Supply
{
    public class SupplyViewModel : BaseViewModel
    {
        public string SupplierName { get; set; }
        public DateTime SupplyDate { get; set; }
        public decimal PriceBrutto { get; set; }
    }
}
