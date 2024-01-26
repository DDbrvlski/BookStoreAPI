using BookStoreViewModels.ViewModels.Customers.Address;

namespace BookStoreViewModels.ViewModels.Supply
{
    public class SupplierPostViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public BaseAddressViewModel Address { get; set; }
    }
}
