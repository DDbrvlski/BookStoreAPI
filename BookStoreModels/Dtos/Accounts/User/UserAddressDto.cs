using BookStoreDto.Dtos.Customers.Address;

namespace BookStoreDto.Dtos.Accounts.User
{
    public class UserAddressDto
    {
        public BaseAddressDto? address { get; set; }
        public BaseAddressDto? mailingAddress { get; set; }
    }
}
