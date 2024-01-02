using BookStoreViewModels.ViewModels.Customers.Address;

namespace BookStoreViewModels.ViewModels.Accounts.User
{
    public class UserAddressViewModel
    {
        public BaseAddressViewModel? address { get; set; }
        public BaseAddressViewModel? mailingAddress { get; set; }
    }
}
