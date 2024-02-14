﻿using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.Customers.Address
{
    public class BaseAddressDto : BaseDto
    {
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string Postcode { get; set; }
        public int CityID { get; set; }
        public int CountryID { get; set; }
        public int AddressTypeID { get; set; }
    }
}