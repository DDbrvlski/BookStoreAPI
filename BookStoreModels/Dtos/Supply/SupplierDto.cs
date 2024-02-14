using BookStoreDto.Dtos.Customers.Address;
using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Supply
{
    public class SupplierDto : BaseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressTypeName { get; set; }
        public AddressDetailsDto SupplierAddress { get; set; }
    }
}
