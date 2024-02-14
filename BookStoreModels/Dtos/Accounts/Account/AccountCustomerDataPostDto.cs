using BookStoreDto.Dtos.Customers.Address;

namespace BookStoreDto.Dtos.Accounts.Account
{
    public class AccountCustomerDataPostDto
    {
        public AddressPostDto? Address { get; set; }
        public AddressPostDto? MailingAddress { get; set; }
    }
}
