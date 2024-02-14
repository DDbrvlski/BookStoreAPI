namespace BookStoreDto.Dtos.Invoices
{
    public class CustomerAddressInvoiceDto
    {
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string Postcode { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
    }
}
