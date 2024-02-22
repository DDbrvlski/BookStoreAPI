namespace BookStoreDto.Dtos.Invoices
{
    public class AdditionalInfoInvoiceDto
    {
        public string PaymentMethodName { get; set; }
        public string CurrencyName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string DeliveryName { get; set; }
        public decimal DeliveryPrice { get; set; }
    }
}
