using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Customers
{
    public class CustomerGuestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
