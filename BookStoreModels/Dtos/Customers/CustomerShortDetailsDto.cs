using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Customers
{
    public class CustomerShortDetailsDto : BaseDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
