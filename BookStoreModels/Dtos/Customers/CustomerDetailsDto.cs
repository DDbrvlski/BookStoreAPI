using BookStoreDto.Dtos.Customers.Address;

namespace BookStoreDto.Dtos.Customers
{
    public class CustomerDetailsDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsSubscribed { get; set; }
        public List<AddressDetailsDto>? ListOfCustomerAdresses { get; set; }
    }
}
