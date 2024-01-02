using BookStoreViewModels.ViewModels.Customers.Address;

namespace BookStoreViewModels.ViewModels.Accounts.Account
{
    public class AccountCustomerDataPostViewModel
    {
        public AddressPostViewModel? Address { get; set; }
        public AddressPostViewModel? MailingAddress { get; set; }
    }
}
