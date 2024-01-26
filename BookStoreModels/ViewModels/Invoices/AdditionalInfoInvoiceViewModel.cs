namespace BookStoreViewModels.ViewModels.Invoices
{
    public class AdditionalInfoInvoiceViewModel
    {
        public string PaymentMethodName { get; set; }
        public string CurrencyName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string DeliveryName { get; set; }
    }
}
