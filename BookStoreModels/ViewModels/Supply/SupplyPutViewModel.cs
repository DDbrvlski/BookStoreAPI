namespace BookStoreViewModels.ViewModels.Supply
{
    public class SupplyPutViewModel
    {
        public int SupplierId { get; set; }
        public int DeliveryStatusId { get; set; }
        public DateTime DeliveryDate { get; set; }

        public List<SupplyBookPostViewModel>? BookItems { get; set; }
    }
}
