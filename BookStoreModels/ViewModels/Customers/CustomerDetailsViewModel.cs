using BookStoreViewModels.ViewModels.Customers.Address;

namespace BookStoreViewModels.ViewModels.Customers
{
    public class CustomerDetailsViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsSubscribed { get; set; }
        public List<AddressDetailsViewModel>? ListOfCustomerAdresses { get; set; }
    }
}
