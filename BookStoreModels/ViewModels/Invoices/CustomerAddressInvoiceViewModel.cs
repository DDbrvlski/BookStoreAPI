namespace BookStoreViewModels.ViewModels.Invoices
{
    public class CustomerAddressInvoiceViewModel
    {
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string Postcode { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
    }
}
