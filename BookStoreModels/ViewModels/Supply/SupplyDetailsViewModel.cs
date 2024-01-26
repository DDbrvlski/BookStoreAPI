using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Payments;

namespace BookStoreViewModels.ViewModels.Supply
{
    public class SupplyDetailsViewModel : BaseViewModel
    {
        public SupplierViewModel SupplierData { get; set; }
        public PaymentDetailsViewModel PaymentData { get; set; }
        public List<SupplyBooksViewModel> SupplyBooksData { get; set; }
    }
}
