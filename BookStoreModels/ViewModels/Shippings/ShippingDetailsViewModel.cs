using BookStoreViewModels.ViewModels.Customers.Address;
using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Shippings.Dictionaries;

namespace BookStoreViewModels.ViewModels.Shippings
{
    public class ShippingDetailsViewModel : BaseViewModel
    {
        public DateTime? ShippingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public AddressDetailsViewModel? ShippingAddress { get; set; }
        public ShippingStatusViewModel? ShippingStatus { get; set; }
    }
}
