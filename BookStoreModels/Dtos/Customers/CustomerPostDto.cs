using BookStoreDto.Dtos.Customers.Address;
using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Customers
{
    public class CustomerPostDto : BaseDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
