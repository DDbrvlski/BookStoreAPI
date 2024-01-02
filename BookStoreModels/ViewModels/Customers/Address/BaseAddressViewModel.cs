using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Customers.Address
{
    public class BaseAddressViewModel : BaseViewModel
    {
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string? Postcode { get; set; }
        public int? CityID { get; set; }
        public int? CountryID { get; set; }
        public int? Position { get; set; }
    }
}
