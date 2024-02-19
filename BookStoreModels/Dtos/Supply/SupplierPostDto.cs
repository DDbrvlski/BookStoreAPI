using BookStoreDto.Dtos.Customers.Address;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Supply
{
    public class SupplierPostDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public BaseAddressDto Address { get; set; }
    }
}
