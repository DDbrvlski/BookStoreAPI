using BookStoreDto.Dtos.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDto.Dtos.Customers.Address
{
    public class BaseAddressDto : BaseDto
    {
        [Required]
        public string Street { get; set; }
        [Required]
        public string StreetNumber { get; set; }
        public string? HouseNumber { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public int CityID { get; set; }
        [Required]
        public int CountryID { get; set; }
        [Required]
        public int AddressTypeID { get; set; }
    }
}
