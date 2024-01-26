using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.Supply
{
    public class SupplyPostViewModel
    {
        public int SupplierId { get; set; }
        public int DeliveryStatusId { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime DeliveryDate { get; set; }

        public List<SupplyBookPostViewModel>? BookItems { get; set; }
    }
}
