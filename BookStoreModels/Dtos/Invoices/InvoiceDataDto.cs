namespace BookStoreDto.Dtos.Invoices
{
    public class InvoiceDataDto
    {
        public int InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public CustomerInvoiceDto CustomerInvoice { get; set; }
        public SellerInvoiceDto SellerInvoice { get; set; }
        public AdditionalInfoInvoiceDto AdditionalInfoInvoice { get; set; }
        public List<ProductInvoiceDto> InvoiceProducts { get; set; }
    }
}
