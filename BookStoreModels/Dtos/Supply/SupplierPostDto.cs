using BookStoreDto.Dtos.Customers.Address;

namespace BookStoreDto.Dtos.Supply
{
    public class SupplierPostDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public BaseAddressDto Address { get; set; }
    }
}
