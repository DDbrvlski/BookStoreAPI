namespace BookStoreViewModels.ViewModels.Invoices
{
    public class InvoiceDataViewModel
    {
        public int InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public CustomerInvoiceViewModel CustomerInvoice { get; set; }
        public SellerInvoiceViewModel SellerInvoice { get; set; }
        public AdditionalInfoInvoiceViewModel AdditionalInfoInvoice { get; set; }
        public List<ProductInvoiceViewModel> InvoiceProducts { get; set; }
    }
}
