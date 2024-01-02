using BookStoreViewModels.ViewModels.Customers.Address;
using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Shippings.Dictionaries
{
    public class ShippingPostViewModel : BaseViewModel
    {
        public DateTime? ShippingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public AddressPostViewModel Address { get; set; }
        public ShippingStatusViewModel? ShippingStatus { get; set; }
    }
}
